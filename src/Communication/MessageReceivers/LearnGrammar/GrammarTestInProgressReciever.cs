using Entities;
using Entities.Common;
using Entities.Navigation;
using Helpers;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace Communication.MessageReceivers.LearnGrammar
{
    public class GrammarTestInProgressReciever : BaseLearnGrammarStateReceiver
    {
        public static UserState State => UserState.GrammarTestInProgress;
        public override string StateInfo => null;

        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return Enumerable.Empty<StateCommand>();
        }
        protected override ActionResult NoCommandAction(Message message, UserItem user)
        {
            return "Пожалуйста, завершите тест".ToActionResult();
        }
    }
}
