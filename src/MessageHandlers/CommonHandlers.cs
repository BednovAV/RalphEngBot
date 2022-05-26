using AuthenticationCore;
using Autofac;
using Communication;
using DataAccessLayer.Interfaces;
using DependencyCore;
using Entities;
using Entities.Common;
using Entities.Navigation;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Handlers
{
    public class CommonHandlers
	{
        public static IContainer Container => AutofacContainer.GetContainer();
        public static IAuthenticationCore AuthenticationCore => Container.Resolve<IAuthenticationCore>();
        public static ITelegramBotClient BotClient => Container.Resolve<ITelegramBotClient>();
        public static IUserDAO UserDAO => Container.Resolve<IUserDAO>();
        public static ICallbackQuerryReciever CallbackQuerryReciever => Container.Resolve<ICallbackQuerryReciever>();
        public static IChatManager ChatManager => Container.Resolve<IChatManager>();
        public static Dictionary<UserState, IMessageReceiver> StrategyByState
            => Enum.GetValues<UserState>().ToDictionary(state => state, state => Container.ResolveKeyed<IMessageReceiver>(state));

        public static Task HandleUpdate(Update update)
            => update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(update.Message),
                UpdateType.EditedMessage => BotOnMessageReceived(update.EditedMessage),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery),
                _ => UnknownUpdateHandlerAsync(update)
            };

        private static async Task BotOnMessageReceived(Message message)
        {
            if (message.Type != MessageType.Text)
                return;

            var user = AuthenticationCore.AuthenticateUser(message.Chat);

            var actionResult = StrategyByState[user.State].Action(message, user);
            await ProcessActionResult(user, actionResult);
        }

        private static async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            var user = AuthenticationCore.AuthenticateUser(callbackQuery.Message.Chat);
            var actionResult = CallbackQuerryReciever.Action(callbackQuery, user);

            await ProcessActionResult(user, actionResult);
            await BotClient.AnswerCallbackQueryAsync(
                callbackQueryId: callbackQuery.Id,
                text: $"Выполнено");
        }

        private static async Task ProcessActionResult(UserItem user, ActionResult actionResult)
        {
            await ChatManager.DeleteMessages(user.Id, actionResult.MessageIdsToDelete);
            await ChatManager.EditMessages(user.Id, actionResult.MessagesToEdit);
            await ChatManager.SendMessages(user.Id, actionResult.MessagesToSend);

            var newState = actionResult.SwitchToUserState;
            if (newState.HasValue)
            {
                UserDAO.SwitchUserState(user.Id, newState.Value);
                var newStateInfo = StrategyByState[newState.Value].StateInfo;
                if (newStateInfo != null && !actionResult.SilentSwitch)
                {
                    await ChatManager.SendMessage(user.Id, newStateInfo.ToMessageData(removeKeyboard: true));
                }
            }
        }

        private static Task UnknownUpdateHandlerAsync(Update update)
        {
            return Task.CompletedTask;
        }
    }
}
