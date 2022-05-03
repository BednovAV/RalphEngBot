using Telegram.Bot.Types.ReplyMarkups;

namespace Entities.Common
{
    public class MessageData
    {
        public string Text { get; set; }
        public IReplyMarkup ReplyMarkup { get; set; }
    }
}
