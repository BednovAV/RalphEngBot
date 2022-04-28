using System;
using System.Threading.Tasks;
using Handlers;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WebClient.Services
{
	public class HandleUpdateService
	{
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<HandleUpdateService> _logger;

        public HandleUpdateService(ITelegramBotClient botClient, ILogger<HandleUpdateService> logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public async Task EchoAsync(Update update)
        {
            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message => CommonHandlers.BotOnMessageReceived(update.Message),
                UpdateType.EditedMessage => CommonHandlers.BotOnMessageReceived(update.EditedMessage),
                UpdateType.CallbackQuery => CommonHandlers.BotOnCallbackQueryReceived(_botClient, update.CallbackQuery),
                UpdateType.InlineQuery => CommonHandlers.BotOnInlineQueryReceived(_botClient, update.InlineQuery),
                UpdateType.ChosenInlineResult => CommonHandlers.BotOnChosenInlineResultReceived(_botClient, update.ChosenInlineResult),
                _ => CommonHandlers.UnknownUpdateHandlerAsync(_botClient, update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }

        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
