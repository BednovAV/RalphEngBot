using DataAccessLayer.Interfaces;
using Entities;
using Entities.Navigation;
using LogicLayer.Interfaces;
using LogicLayer.StateStrategy.Common;
using System.Collections.Generic;

namespace LogicLayer.StateStrategy
{
    public class LearningStrategy : BaseStateStrategy
    {
        public static UserState State => UserState.LearnWordsMode;

        private readonly IWordsLogic _wordsLogic;

        public LearningStrategy(IUserDAO userDAO, IWordsLogic wordsLogic) : base(userDAO)
        {
            _wordsLogic = wordsLogic;
        }

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
