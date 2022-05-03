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
            if (removeKeyboard)
                replyMarkup = new ReplyKeyboardRemove();

            return _client.SendTextMessageAsync(chatId: userId,
                                                text: GetMarkdownText(text),
                                                parseMode: ParseMode.MarkdownV2,
                                                replyMarkup: replyMarkup);
        }
        public static Task<Message> SendMessage(this ITelegramBotClient _client, long userId, MessageData message)
        {
            return _client.SendMessage(userId, message.Text, message.ReplyMarkup == null, message.ReplyMarkup);
        }

        public static async Task SendMessage(this ITelegramBotClient _client, long userId, IEnumerable<MessageData> messages)
        {
            var deleteKeyboard = !messages.Any(x => x.ReplyMarkup != null);
            foreach (var message in messages)
            {
                await _client.SendMessage(userId, message.Text, deleteKeyboard, message.ReplyMarkup);
            }
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
