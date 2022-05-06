using Entities.Common;
using Entities.Navigation;
using Telegram.Bot.Types;

namespace Communication
{
    public interface IMessageReceiver
    {
        public ActionResult Action(Message message, UserItem user);
        public string StateInfo { get; }
    }
}
