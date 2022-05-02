using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.Navigation;
using LogicLayer.Interfaces;
using LogicLayer.StateStrategy.Common;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy
{
    public class WaitingNewWordStrategy : BaseStateStrategy
    {
        public static UserState State => UserState.WaitingNewWord;

        private readonly IWordsLogic _wordsLogic;

        public WaitingNewWordStrategy(IUserDAO userDAO, IWordsLogic wordsLogic) : base(userDAO)
        {
            _wordsLogic = wordsLogic;
        }

        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new[]
            {
                BackToMainCommand,
            };
        }

        protected override IEnumerable<MessageData> NoCommandAction(Message message, UserItem user)
            => _wordsLogic.SelectWord(message, user);
    }
}
