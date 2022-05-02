using Entities.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LogicLayer.Interfaces
{
    public interface IWordsLogic
    {
        IEnumerable<MessageData> LearnWords(UserItem user);
        IEnumerable<MessageData> SelectWord(Message message, UserItem user);
        IEnumerable<MessageData> ProcessWordResponse(Message message, UserItem user);
    }
}
