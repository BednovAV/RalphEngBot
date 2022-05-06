using DataAccessLayer.Interfaces;
using Entities.Navigation;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Words;

namespace Communication
{
    public abstract class BaseLearnWordsStateReceiver : BaseMessageReceiver
    {
        protected readonly IWordsLogic _wordsLogic;
        protected readonly IWordsAccessor _wordsAccessor;


        protected BaseLearnWordsStateReceiver(IUserDAO userDAO, IWordsLogic wordsLogic, IWordsAccessor wordsAccessor) : base(userDAO)
        {
            _wordsLogic = wordsLogic;
            _wordsAccessor = wordsAccessor;
        }

        protected StateCommand BackToLearnCommand => new StateCommand
        {
            Key = "/back",
            Description = "Выйти",
            Execute = (message, user) => _wordsLogic.StopLearn(user)
        };
    }
}
