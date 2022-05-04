using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.Navigation;
using LogicLayer.Interfaces;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Receivers
{
    public class WaitingNewWordStrategy : BaseLearnWordsStateStrategy
    {
        public static UserState State => UserState.WaitingNewWord;

        public WaitingNewWordStrategy(IUserDAO userDAO, IWordsLogic wordsLogic) : base(userDAO, wordsLogic)
        {
        }

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
