using SeaBattleTelegramBot.Commands;
using SeaBattleTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using static SeaBattleTelegramBot.Models.SeaBattleAdjustments;

namespace SeaBattleTelegramBot.Services
{
    public class UpdateManager
    {
        static IReadOnlyList<Command> Commands = null;
        static DBService _db;

        public static async Task Handle(ITelegramBotClient arg1, Update update, CancellationToken cancellationToken, IReadOnlyList<Command> commands, TelegramBotClient client, DBService db)
        {

            _db = db;
            Commands = commands;

            switch (update.Type)
            {

                case UpdateType.Message:
                    try
                    {
                        await HandleMessage(update, client);
                    }
                    catch (Exception e)
                    {

                        await HandleErrorAsync(client, e, cancellationToken);
                    }
                    break;
            }
        }
        private static async Task HandleMessage(Update update, TelegramBotClient client)
        {
            Message message = update.Message;
            switch (message.Type)
            {
                case MessageType.Text:
                    await ManageText(update, client);
                    break;
            }
        }

        private static async Task ManageText(Update update, TelegramBotClient client)
        {
            Message message = update.Message;

            var commands = Commands;

            Models.User user = _db.FindUser(message.From.Id);
            if (user == null)
            {
                user = new()
                { UserId = message.From.Id, GameId = null, Ships = SeaBattleAdjustments.GetShips(), Status = (int)SeaBattleAdjustments.Status.NotInGame};
                await _db.AddUserAsync(user);
            }

            foreach (var command in commands)
            {
                if (command.Contains(message.Text))
                {
                    try
                    {
                        await command.Execute(message, client);
                        return;
                    }
                    catch
                    {

                    }
                }
            }

            // Moves handling
            List<string> letters = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            List<string> numbers = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

            string[] move = message.Text.Split(" ");

            if (user.Status != (int)Status.MyTurn)
            {
                await client.SendTextMessageAsync(message.From.Id, "Не ваш ход!\n");
                return;
            }
            if(move.Length<2 || !letters.Contains(move[0].ToLower()) || !numbers.Contains(move[1]) || move[0].Length>1 || move[1].Length > 1)
            {
                await client.SendTextMessageAsync(message.From.Id, "Неверный ввод, введите букву и цифру через пробел\n");
                return;
            }
            else
            {
                int J =0, I = 0;
                PrepareMove(move[0], move[1], ref I, ref J);

                Models.User opponent = _db.FindUserByGameId(user.UserId, user.GameId);
                int[][] ships = SplitShips(ReplaceShipsSymbolsToNumber(opponent.Ships));
                int[][] enemyShips = SplitShips(ReplaceShipsSymbolsToNumber(user.EnemyField));
                //Shot
                if (ships[I][J] == 1)
                {
                    ships[I][J] = 2;
                    enemyShips[I][J] = 2;

                    await _db.SetEnemyField(user.UserId, ReplaceShipsNumberToSymbols(JoinShips(enemyShips)));
                    await _db.SetShips(opponent.UserId, ReplaceShipsNumberToSymbols(JoinShips(ships)));

                    string enemyField = ToDisplay(ReplaceShipsNumberToSymbols(JoinShips(enemyShips)));
                    string field = ToDisplay(ReplaceShipsNumberToSymbols(JoinShips(ships)));

                    bool win = true;
                    for(int i = 0; i < ships.Length; i++)
                    {
                        for(int j = 0; j < ships[i].Length; j++)
                        {
                            if (ships[i][j] == 1) win = false;
                        }
                    }

                    if (win)
                    {
                        await _db.SetStatus(user.UserId, Status.NotInGame);
                        await _db.SetStatus(opponent.UserId, Status.NotInGame);
                        await client.SendTextMessageAsync(message.From.Id, "Вы выиграли \nПоле противника:\n" + enemyField, parseMode: ParseMode.Html);
                        await client.SendTextMessageAsync(opponent.UserId, "Вы проиграли\nВаши корабли:\n" + field, parseMode: ParseMode.Html);
                        return;
                    }

                    await client.SendTextMessageAsync(message.From.Id, "Попадание, ходите снова\nПоле противника:\n" + enemyField, parseMode: ParseMode.Html);
                    await client.SendTextMessageAsync(opponent.UserId, "По вам попадание, ход противника\nВаши корабли:\n"+field, parseMode: ParseMode.Html);
                }
                else
                {
                    ships[I][J] = 3;
                    enemyShips[I][J] = 3;

                    await _db.SetEnemyField(user.UserId, ReplaceShipsNumberToSymbols(JoinShips(enemyShips)));
                    await _db.SetShips(opponent.UserId, ReplaceShipsNumberToSymbols(JoinShips(ships)));

                    string enemyField = ToDisplay(ReplaceShipsNumberToSymbols(JoinShips(enemyShips)));
                    string field = ToDisplay(ReplaceShipsNumberToSymbols(JoinShips(ships)));

                    await _db.SetStatus(user.UserId, Status.Wait);
                    await _db.SetStatus(opponent.UserId, Status.MyTurn);

                    await client.SendTextMessageAsync(message.From.Id, "Промах, ход противника\nПоле противника:\n" + enemyField, parseMode: ParseMode.Html);
                    await client.SendTextMessageAsync(opponent.UserId, "Противник промахнулся, теперь ваш ход\nВаши корабли:\n" + field, parseMode: ParseMode.Html);
                    
                }

            }
            

        }

        public static Task HandleErrorAsync(ITelegramBotClient arg1, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine(exception.ToString());
            return Task.CompletedTask;
        }
    }
}
