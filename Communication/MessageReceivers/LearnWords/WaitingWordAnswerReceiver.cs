using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.Navigation;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Words;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Communication
{
    public class WaitingWordAnswerReceiver : BaseLearnWordsStateReceiver
    {
        public WaitingWordAnswerReceiver(IWordsLogic wordsLogic, IWordsAccessor wordsAccessor) : base(wordsLogic, wordsAccessor)
        {
        }

        public static UserState State => UserState.WaitingWordResponse;


        public override string StateInfo => null;

        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new[]
            {
                BackToLearnCommand,
            };
        }

        protected override ActionResult NoCommandAction(Message message, UserItem user) 
            => _wordsLogic.ProcessWordResponse(message, user);
    }
}
