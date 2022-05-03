using DataAccessLayer.Interfaces;
using Entities;
using Entities.Navigation;
using LogicLayer.Interfaces;
using LogicLayer.StateStrategy.Common;
using System.Collections.Generic;

namespace LogicLayer.StateStrategy
{
    public class LearningStrategy : BaseLearnWordsStateStrategy
    {
        public static UserState State => UserState.LearnWordsMode;


        public LearningStrategy(IUserDAO userDAO, IWordsLogic wordsLogic) : base(userDAO, wordsLogic)
        {
        }

        public override string StateInfo => "*Режим изучения слов* 👨‍🎓\n" + GetCommandsDescriptions();


        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new StateCommand[]
            {
                StartLearnCommand,
                BackToMainCommand
            };
        }

        private StateCommand StartLearnCommand => new StateCommand
        {
            Key = "/startlearn",
            Description = "начать изучение слов",
            Execute = (message, user) => _wordsLogic.LearnWords(user)
        };
    }
}
