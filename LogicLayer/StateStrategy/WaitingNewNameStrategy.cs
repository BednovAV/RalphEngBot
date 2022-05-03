using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.Navigation;
using Helpers;
using LogicLayer.StateStrategy.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy
{
    public class WaitingNewNameStrategy : BaseStateStrategy
    {
        public static UserState State => UserState.WaitingNewName;

        public WaitingNewNameStrategy(IUserDAO userDAO) : base(userDAO)
        {
        }

        public override string StateInfo => null;

        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new[]
            {
                BackToMainCommand
            };
        }

        protected override ActionResult NoCommandAction(Message message, UserItem user)
        {
            user.Name = message.Text;
            _userDAO.Update(user);
            return $"Теперь я буду называть вас *{message.Text}*".ToActionResult(UserState.WaitingCommand);
        }
    }
}
