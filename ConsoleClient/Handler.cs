using System;
using Handlers;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ConsoleClient
{
	public class Handler
	{
        public static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => CommonHandlers.BotOnMessageReceived(botClient, update.Message),
                UpdateType.EditedMessage => CommonHandlers.BotOnMessageReceived(botClient, update.EditedMessage),
                UpdateType.CallbackQuery => CommonHandlers.BotOnCallbackQueryReceived(botClient, update.CallbackQuery),
                UpdateType.InlineQuery => CommonHandlers.BotOnInlineQueryReceived(botClient, update.InlineQuery),
                UpdateType.ChosenInlineResult => CommonHandlers.BotOnChosenInlineResultReceived(botClient, update.ChosenInlineResult),
                _ => CommonHandlers.UnknownUpdateHandlerAsync(botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(botClient, exception, cancellationToken);
            }
        }
    }
}
