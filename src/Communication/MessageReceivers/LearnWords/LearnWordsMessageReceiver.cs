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
        public LearnWordsMessageReceiver(ILearnWordsLogic learnWordsLogic, IRepetitionWordsLogic repetitionWordsLogic, IWordsAccessor wordsAccessor) : base(learnWordsLogic, repetitionWordsLogic, wordsAccessor)
        {
        }

        public static UserState State => UserState.LearnWordsMode;

        public override string StateInfo => "*Изучение слов* 🧠\n" + GetCommandsDescriptions();


        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new StateCommand[]
            {
                StartLearnCommand,
                StartRepetitionCommand,
                MyWordsCommand,
                BackToMainCommand
            };
        }

        private StateCommand StartLearnCommand => new StateCommand
        {
            Key = "/startlearn",
            Description = "начать изучение слов",
            Execute = (message, user) => _learnWordsLogic.StartLearnWords(user)
        };
        private StateCommand StartRepetitionCommand => new StateCommand
        {
            Key = "/startrepetition",
            Description = "повторить изученные слова",
            Execute = (message, user) => _repetitionWordsLogic.StartRepetition(user)
        };
        private StateCommand MyWordsCommand => new StateCommand
        {
            Key = "/mywords",
            Description = "посмотреть список изученных слов",
            Execute = (message, user) => _wordsAccessor.ShowUserWords(user)
        };
    }
}
