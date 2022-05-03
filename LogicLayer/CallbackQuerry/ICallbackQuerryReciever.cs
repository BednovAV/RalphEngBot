using Entities.Common;
using Entities.Navigation;
using Telegram.Bot.Types;

namespace LogicLayer.CallbackQuerry
{
    public interface ICallbackQuerryReciever
    {
        ActionResult Action(CallbackQuery callbackQuery, UserItem user);
    }
}
