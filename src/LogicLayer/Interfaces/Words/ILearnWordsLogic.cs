using Entities.Common;
using Entities.Navigation;
using Telegram.Bot.Types;

namespace LogicLayer.Interfaces
{
    public interface ILearnWordsLogic
    {
        ActionResult SelectWord(Message message, UserItem user);
        ActionResult ProcessWordResponse(Message message, UserItem user);
        ActionResult StartLearnWords(UserItem user);
        ActionResult HintWord(UserItem user);
        ActionResult StopWordsAction(UserItem user);
    }
}
