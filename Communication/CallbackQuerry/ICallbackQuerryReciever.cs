using Entities.Common;
using Entities.Navigation;
using Telegram.Bot.Types;

namespace Communication
{
    public interface ICallbackQuerryReciever
    {
        ActionResult Action(CallbackQuery callbackQuery, UserItem user);
    }
}
