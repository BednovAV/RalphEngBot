using Entities.Common;
using Entities.Navigation;
using Telegram.Bot.Types;

namespace LogicLayer.Interfaces.Words
{
    public interface IRepetitionWordsLogic
    {
        ActionResult ProcessWordResponse(Message message, UserItem user);
        ActionResult StartRepetition(UserItem user);
        ActionResult HintWord(UserItem user);
        ActionResult StopWordsAction(UserItem user);
    }
}
