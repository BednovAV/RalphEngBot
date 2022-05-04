using Entities.Common;
using Entities.Navigation;
using Telegram.Bot.Types;

namespace Receivers
{
    public interface ICallbackQuerryReciever
    {
        ActionResult Action(CallbackQuery callbackQuery, UserItem user);
    }
}
