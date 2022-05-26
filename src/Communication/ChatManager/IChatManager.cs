using Entities.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Communication
{
    public interface IChatManager
    {
        Task DeleteMessages(long userId, IEnumerable<int> deleteMessageIds);
        Task<Message> EditMessage(long userId, EditMessageData message);
        Task EditMessages(long userId, IEnumerable<EditMessageData> messages);
        Task<Message> SendMessage(long userId, MessageData message);
        Task SendMessages(long userId, IEnumerable<MessageData> messages);
    }
}
