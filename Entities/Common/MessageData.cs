using Telegram.Bot.Types.ReplyMarkups;

namespace Entities.Common
{
    public class MessageData
    {
        public string Text { get; set; }
        public bool RemoveKeyboard { get; set; } = true;
        public IReplyMarkup ReplyMarkup { get; set; }
    }
}
