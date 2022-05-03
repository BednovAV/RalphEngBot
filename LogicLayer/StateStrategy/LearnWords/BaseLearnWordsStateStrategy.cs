using DataAccessLayer.Interfaces;
using Entities.Navigation;
using LogicLayer.Interfaces;
using LogicLayer.StateStrategy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.StateStrategy
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
