using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Helpers
{
    public static class BotClientHelper
    {
        public static Task<Message> SendMessage(this ITelegramBotClient _client, long userId, string text, string[] replyKeyboardData = null)
        {
            IReplyMarkup replyMarkup = null;

            if (replyKeyboardData?.Any() is true)
            {
                replyMarkup = new ReplyKeyboardMarkup(replyKeyboardData.Select(x => new KeyboardButton(x)).ToArray())
                {
                    ResizeKeyboard = true,
                };
            }
            return _client.SendTextMessageAsync(chatId: userId,
                                                text: text,
                                                replyMarkup: replyMarkup);
        }
    }
}
