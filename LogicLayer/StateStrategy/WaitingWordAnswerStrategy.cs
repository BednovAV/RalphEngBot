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
    public class WaitingWordAnswerStrategy : BaseStateStrategy
    {
        public static UserState State => UserState.WaitingWordResponse;

        private readonly IWordsLogic _wordsLogic;

        public WaitingWordAnswerStrategy(IUserDAO userDAO, IWordsLogic wordsLogic) : base(userDAO)
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
            => _wordsLogic.ProcessWordResponse(message, user);
    }
}
