using DataAccessLayer.Interfaces;
using Entities.Navigation;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Words;

namespace Communication
{
    public abstract class BaseLearnWordsStateReceiver : BaseMessageReceiver
    {
        protected readonly ILearnWordsLogic _learnWordsLogic;
        protected readonly IRepetitionWordsLogic _repetitionWordsLogic;
        protected readonly IWordsAccessor _wordsAccessor;

        protected BaseLearnWordsStateReceiver(ILearnWordsLogic learnWordsLogic, IRepetitionWordsLogic repetitionWordsLogic, IWordsAccessor wordsAccessor)
        {
            _learnWordsLogic = learnWordsLogic;
            _repetitionWordsLogic = repetitionWordsLogic;
            _wordsAccessor = wordsAccessor;
        }

        protected StateCommand BackToLearnCommand => new StateCommand
        {
            Key = "/back",
            Description = "Выйти",
            Execute = (message, user) => _learnWordsLogic.StopWordsAction(user)
        };
    }
}
