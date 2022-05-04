using DataAccessLayer.Interfaces;
using Entities.Navigation;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Words;

namespace Receivers
{
    public abstract class BaseLearnWordsStateStrategy : BaseStateStrategy
    {
        protected readonly IWordsLogic _wordsLogic;
        protected readonly IWordsAccessor _wordsAccessor;


        protected BaseLearnWordsStateStrategy(IUserDAO userDAO, IWordsLogic wordsLogic, IWordsAccessor wordsAccessor) : base(userDAO)
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
