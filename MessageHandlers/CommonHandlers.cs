using AuthenticationCore;
using Autofac;
using DataAccessLayer.Interfaces;
using DependencyCore;
using Entities;
using Helpers;
using LogicLayer.StateStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;

namespace Handlers
{
    public class CommonHandlers
	{
        public static IContainer Container => AutofacContainer.GetContainer();
        public static IAuthenticationCore AuthenticationCore => Container.Resolve<IAuthenticationCore>();
        public static ITelegramBotClient BotClient => Container.Resolve<ITelegramBotClient>();
        public static Dictionary<UserState, IStateStrategy> StrategyByState
            => Enum.GetValues<UserState>().ToDictionary(state => state, state => Container.ResolveKeyed<IStateStrategy>(state));

        public static async Task BotOnMessageReceived(Message message)
        {
            if (message.Type != MessageType.Text)
                return;

            var user = AuthenticationCore.AuthenticateUser(message.Chat);
            var messages = StrategyByState[user.State].Action(message, user);

            foreach (var msg in messages)
            {
                await BotClient.SendMessage(user.Id, msg);
            }
        }

        // Process Inline Keyboard callback data
        public static async Task BotOnCallbackQueryReceived(ITelegramBotClient botClient, CallbackQuery callbackQuery)
        {
            await botClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Received {callbackQuery.Data}");

            await botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message.Chat.Id,
                text: $"Received {callbackQuery.Data}");
        }

        public static async Task BotOnInlineQueryReceived(ITelegramBotClient botClient, InlineQuery inlineQuery)
        {
            InlineQueryResultBase[] results = {
                // displayed result
                new InlineQueryResultArticle(
                    id: "3",
                    title: "TgBots",
                    inputMessageContent: new InputTextMessageContent(
                        "hello"
                    )
                )
            };

            await botClient.AnswerInlineQueryAsync(
                inlineQueryId: inlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 0);
        }

        public static Task BotOnChosenInlineResultReceived(ITelegramBotClient botClient, ChosenInlineResult chosenInlineResult)
        {
            return Task.CompletedTask;
        }

        public static Task UnknownUpdateHandlerAsync(ITelegramBotClient botClient, Update update)
        {
            return Task.CompletedTask;
        }
    }
}
