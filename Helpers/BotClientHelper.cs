using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Helpers
{
    public static class BotClientHelper
    {

        public static Task<Message> SendMessage(this ITelegramBotClient _client, long userId, string text, IReplyMarkup replyMarkup = null)
        {
            replyMarkup ??= new ReplyKeyboardRemove();

            return _client.SendTextMessageAsync(chatId: userId,
                                                text: GetMarkdownText(text),
                                                parseMode: ParseMode.MarkdownV2,
                                                replyMarkup: replyMarkup);
        }

        private static string GetMarkdownText(string text)
        {
            var textBuilder = new StringBuilder(text);
            for (int i = 0; i < textBuilder.Length; i++)
            {
                if (textBuilder[i] is '!' or '(' or ')' or '-' or '.')
                {
                    textBuilder.Insert(i, '\\');
                    i++;
                }
            }

            return textBuilder.ToString();
        }
    }
}
