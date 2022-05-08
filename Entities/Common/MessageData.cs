using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Entities.Common
{
    public class MessageData
    {
        public string Text { get; set; }
        public IReplyMarkup ReplyMarkup { get; set; }
        public bool RemoveKeyboard { get; set; }
        public ParseMode ParseMode { get; set; } = ParseMode.Markdown;
    }
}
