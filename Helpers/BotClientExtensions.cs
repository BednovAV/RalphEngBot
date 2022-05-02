using Entities.Common;
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
    public static class BotClientExtensions
    {

        public static Task<Message> SendMessage(this ITelegramBotClient _client, 
            long userId,
            string text,
            bool removeKeyboard,
            IReplyMarkup replyMarkup = null)
        {
            if (removeKeyboard && replyMarkup is null)
                replyMarkup = new ReplyKeyboardRemove();

            return _client.SendTextMessageAsync(chatId: userId,
                                                text: GetMarkdownText(text),
                                                parseMode: ParseMode.MarkdownV2,
                                                replyMarkup: replyMarkup);
        }
        public static Task<Message> SendMessage(this ITelegramBotClient _client, long userId, MessageData message)
        {
            return _client.SendMessage(userId, message.Text, message.RemoveKeyboard, message.ReplyMarkup);
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
