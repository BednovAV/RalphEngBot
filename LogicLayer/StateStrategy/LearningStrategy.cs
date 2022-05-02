using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Helpers;
using LogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy
{
    public class LearningStrategy : IStateStrategy
    {
        private readonly IUserDAO _userDAO;
        private readonly IWordsLogic _wordsLogic;

        public LearningStrategy(IUserDAO userDAO, IWordsLogic wordsLogic)
        {
            _userDAO = userDAO;
            _wordsLogic = wordsLogic;
        }

        public static UserState State => UserState.LearnWordsMode;

        public IEnumerable<MessageData> Action(Message message, UserItem user)
        {
            return message.Text.Split(' ').First() switch
            {
                "/back" => Back(user),
                "/startlearn" => _wordsLogic.LearnWords(user),
                _ => Usage(message)
            };
        }

        public MessageData[] Usage(Message message)
        {
            string usage = "Доступные команды:\n" +
                           "/startlearn - начать изучение слов\n" +
                           "/back - выйти";

            return new MessageData[] { usage.ToMessageData() };
        }

        private MessageData[] Back(UserItem user)
        {
            _userDAO.SwitchUserState(user.Id, UserState.WaitingCommand);
            return new MessageData[] { "Режим изучения слов выключен.\n/help - список доступных команд".ToMessageData() };
        }
    }
}
