using Entities.Common;
using Entities.Navigation;
using Telegram.Bot.Types;

namespace Receivers
{
    public interface IStateStrategy
    {
        public ActionResult Action(Message message, UserItem user);
        public string StateInfo { get; }
    }
}
