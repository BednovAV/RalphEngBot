using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy
{
    public class WaitingNewNameStrategy : IStateStrategy
    {
        private readonly IUserDAO _userDAO;
        private readonly ITelegramBotClient _botClient;

        public WaitingNewNameStrategy(ITelegramBotClient botClient, IUserDAO userDAO)
        {
            _botClient = botClient;
            _userDAO = userDAO;
        }

        public static UserState State => UserState.WaitingNewName;

        public IEnumerable<MessageData> Action(Message message, UserItem user)
        {
            user.State = UserState.WaitingCommand;
            user.Name = message.Text;
            _userDAO.Update(user);
            return new MessageData[] { $"Теперь я буду называть вас *{message.Text}*".ToMessageData() };
        }
    }
}
