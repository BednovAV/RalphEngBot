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
    public class WaitingLearnWordAnswerReceiver : BaseLearnWordsStateReceiver
    {
        public WaitingLearnWordAnswerReceiver(ILearnWordsLogic learnWordsLogic, IRepetitionWordsLogic repetitionWordsLogic, IWordsAccessor wordsAccessor) : base(learnWordsLogic, repetitionWordsLogic, wordsAccessor)
        {
        }

        public static UserState State => UserState.WaitingLearnWordResponse;


        public override string StateInfo => null;

        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new[]
            {
                BackToLearnCommand,
            };
        }

        protected override ActionResult NoCommandAction(Message message, UserItem user) 
            => _learnWordsLogic.ProcessWordResponse(message, user);
    }
}
