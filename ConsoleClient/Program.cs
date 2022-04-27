using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;

namespace ConsoleClient
{
	class Program
	{
        private static TelegramBotClient? Bot;
        public static IConfiguration Configuration => BuildConfiguration();

        static async Task Main(string[] args)
		{
            var botConfig = Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
            Bot = new TelegramBotClient(botConfig.Token);
            
            var me = await Bot.GetMeAsync();
            Console.Title = me.Username;

            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            Bot.StartReceiving(new DefaultUpdateHandler(Handler.HandleUpdateAsync, Handler.HandleErrorAsync),
                               cts.Token);

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}
