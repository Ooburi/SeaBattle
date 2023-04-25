using SeaBattleTelegramBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using static SeaBattleTelegramBot.Models.SeaBattleAdjustments;

namespace SeaBattleTelegramBot.Commands
{
    public class StartCommand : Command
    {
        DBService _db;
        public StartCommand(DBService db)
        {
            _db = db;
        }
        public override string Name => "/start";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            Models.User user = _db.FindUser(message.From.Id);
            Models.User op;

            string text = ToDisplay(user.Ships);

            switch (user.Status)
            {
                case (int)Status.NotInGame:
                    text = "Я бот для игры Морской бой!\n" +
                        "Выполняется поиск партнёра для игры\n" +
                        "Когда он найдётся, я сообщу\n" +
                        "Вот ваше поле с кораблями\n" + text;
                    await client.SendTextMessageAsync(user.UserId, text, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                    op = _db.FindFree(message.From.Id);
                    if (op != null)
                    {
                        
                        string gameId = Guid.NewGuid().ToString();

                        await _db.SetEnemyField(user.UserId, GetEmptyField());
                        await _db.SetStatus(user.UserId, Status.MyTurn);
                        await _db.SetGameId(user.UserId, gameId);

                        await _db.SetEnemyField(op.UserId, GetEmptyField());
                        await _db.SetStatus(op.UserId, Status.Wait);
                        await _db.SetGameId(op.UserId, gameId);

                        user = _db.FindUser(message.From.Id);
                        string emptyField = ToDisplay(user.EnemyField);

                        string myMsg = "Найден противник, сейчас ваш ход\nВот поле противника:\n" + emptyField + "\nВведите букву и цифру хода через пробел, например:\nA 2";
                        string opMsg = "Найден противник, сейчас его ход, ожидайте\nВот поле противника:\n" + emptyField;
                        await client.SendTextMessageAsync(user.UserId, myMsg, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                        await client.SendTextMessageAsync(op.UserId, opMsg, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                    }
                    else
                    {

                    }
                    break;
                default:
                    op = _db.FindUserByGameId(user.UserId,user.GameId);
                    await _db.SetStatus(user.UserId, Status.NotInGame);
                    await _db.SetGameId(user.UserId, "");
                    await _db.SetStatus(op.UserId, Status.NotInGame);
                    await _db.SetGameId(op.UserId, "");
                    await client.SendTextMessageAsync(op.UserId, "Оппонент вышел из игры\nИщем нового противника...");
                    break;
            }
        }
    }
}
