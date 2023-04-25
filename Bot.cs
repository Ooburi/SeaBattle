using SeaBattleTelegramBot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;

namespace SeaBattleTelegramBot.Services
{
    class Bot
    {
        private static TelegramBotClient client;

        private static List<Command> commandsList;
        public static IReadOnlyList<Command> Commands { get => commandsList.AsReadOnly(); }

        private static DBService _db;

        public static async Task<TelegramBotClient> Get(DBService db)
        {
            _db = db;

            if (client != null)
            {
                return client;
            }

            // await client.SetMyCommandsAsync(Resources.Commands.botCommands);

            commandsList = new List<Command>
            {
                    new StartCommand(_db)
            };


            client = new TelegramBotClient(BotSettings.Key);
            var cts = new CancellationTokenSource();
            await client.ReceiveAsync(new DefaultUpdateHandler(updateHandler: HandleUpdateAsync, errorHandler: UpdateManager.HandleErrorAsync), cancellationToken: cts.Token);

            return client;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient arg1, Update update, CancellationToken cancellationToken)
        {
            await UpdateManager.Handle(arg1, update, cancellationToken, Commands, client, _db);
        }
    }
}
