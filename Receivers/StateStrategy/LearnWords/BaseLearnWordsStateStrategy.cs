using DataAccessLayer.Interfaces;
using Entities.Navigation;
using LogicLayer.Interfaces;

namespace Receivers
{
    public abstract class BaseLearnWordsStateStrategy : BaseStateStrategy
    {
        protected readonly IWordsLogic _wordsLogic;


        protected BaseLearnWordsStateStrategy(IUserDAO userDAO, IWordsLogic wordsLogic) : base(userDAO)
        {
            _wordsLogic = wordsLogic;
        }

        protected StateCommand BackToLearnCommand => new StateCommand
        {
            Key = "/back",
            Description = "Выйти",
            Execute = (message, user) => _wordsLogic.StopLearn(user)
        };
    }
}
