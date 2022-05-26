using Entities;
using Entities.Common;
using Entities.Navigation;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Words;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Communication
{
    public class WaitingRepeatWordAnswerReceiver : BaseLearnWordsStateReceiver
    {
        public WaitingRepeatWordAnswerReceiver(ILearnWordsLogic learnWordsLogic, IRepetitionWordsLogic repetitionWordsLogic, IWordsAccessor wordsAccessor) : base(learnWordsLogic, repetitionWordsLogic, wordsAccessor)
        {
        }

        public static UserState State => UserState.WaitingRepetitionWordResponse;

        public override string StateInfo => null;

        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new[]
            {
                BackToLearnCommand,
            };
        }

        protected override ActionResult NoCommandAction(Message message, UserItem user)
            => _repetitionWordsLogic.ProcessWordResponse(message, user);
    }
}
