using Entities.Common;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LogicLayer.Interfaces
{
    public interface IWordsLogic
    {
        Task<Message> LearnWords(UserItem user);
        Task<Message> SelectWord(Message message, UserItem user);
        Task<Message> ProcessWordResponse(Message message, UserItem user);
    }
}
