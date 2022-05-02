using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using LogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy
{
    public class WaitingNewWordStrategy : IStateStrategy
    {
        private IWordsLogic _wordsLogic;
        private IUserDAO _userDAO;

        public WaitingNewWordStrategy(IWordsLogic wordsLogic, IUserDAO userDAO)
        {
            _wordsLogic = wordsLogic;
            _userDAO = userDAO;
        }

        public static UserState State => UserState.WaitingNewWord;

        public IEnumerable<MessageData> Action(Message message, UserItem user)
        {
            return message.Text.Split(' ').First() switch
            {
                "/stop" => StopWaiting(user),
                _ => _wordsLogic.SelectWord(message, user)
            };
        }

        private IEnumerable<MessageData> StopWaiting(UserItem user)
        {
            _userDAO.SwitchUserState(user.Id, UserState.WaitingCommand);
            return Enumerable.Empty<MessageData>();
        }
    }
}
