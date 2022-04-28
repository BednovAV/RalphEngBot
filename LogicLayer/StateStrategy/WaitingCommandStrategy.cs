using DataAccessLayer.Interfaces;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogicLayer.StateStrategy
{
    public class WaitingCommandStrategy : IStateStrategy
    {
        private readonly IUserDAO _userDAO;
        private readonly ITelegramBotClient _botClient;

        public WaitingCommandStrategy(IUserDAO userDAO, ITelegramBotClient botClient)
        {
            _userDAO = userDAO;
            _botClient = botClient;
        }

        public static UserState State => UserState.WaitingCommand;

        public Task Action(Message message, Entities.User user)
        {
            return message.Text.Split(' ').First() switch
            {
                "/rename" => RenameUser(message, user),
                _ => Usage(message, user)
            };
        }

        private Task<Message> RenameUser(Message message, Entities.User user)
        {
            user.State = Entities.UserState.WaitingNewName;
            _userDAO.Update(user);

            return _botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                   text: "How can I contact you?");
        }

        public Task<Message> Usage(Message message, Entities.User user)
        {
            string usage = $"Hello, {user.Name}.\n" +
                           $"I can:\n" +
                            "/rename   - change call";

            return _botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: usage,
                                                        replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
