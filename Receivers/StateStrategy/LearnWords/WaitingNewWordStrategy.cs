using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.Navigation;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Words;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Receivers
{
    public class WaitingNewWordStrategy : BaseLearnWordsStateStrategy
    {
        public WaitingNewWordStrategy(IUserDAO userDAO, IWordsLogic wordsLogic, IWordsAccessor wordsAccessor) : base(userDAO, wordsLogic, wordsAccessor)
        {
        }

        public static UserState State => UserState.WaitingNewWord;

        public override string StateInfo => null;

        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new[]
            {
                BackToLearnCommand,
            };
        }

        protected override ActionResult NoCommandAction(Message message, UserItem user)
            => _wordsLogic.SelectWord(message, user);
    }
}
