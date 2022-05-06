using DataAccessLayer.Interfaces;
using Entities;
using Entities.Navigation;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Words;
using System.Collections.Generic;

namespace Communication
{
    public class LearnWordsMessageReceiver : BaseLearnWordsStateReceiver
    {
        public LearnWordsMessageReceiver(IWordsLogic wordsLogic, IWordsAccessor wordsAccessor) : base(wordsLogic, wordsAccessor)
        {
        }

        public static UserState State => UserState.LearnWordsMode;

        public override string StateInfo => "*Изучение слов* 🧠\n" + GetCommandsDescriptions();


        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new StateCommand[]
            {
                StartLearnCommand,
                MyWordsCommand,
                BackToMainCommand
            };
        }

        private StateCommand StartLearnCommand => new StateCommand
        {
            Key = "/startlearn",
            Description = "начать изучение слов",
            Execute = (message, user) => _wordsLogic.StartLearnWords(user)
        };
        private StateCommand MyWordsCommand => new StateCommand
        {
            Key = "/mywords",
            Description = "посмотреть список изученных слов",
            Execute = (message, user) => _wordsAccessor.ShowUserWords(user)
        };
    }
}
