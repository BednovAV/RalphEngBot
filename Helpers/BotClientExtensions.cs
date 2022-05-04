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
            IReplyMarkup replyMarkup = null)
        {
            return _client.SendTextMessageAsync(chatId: userId,
                                                text: GetMarkdownText(text),
                                                parseMode: ParseMode.MarkdownV2,
                                                replyMarkup: replyMarkup);
        }
        public static Task<Message> EditMessage(this ITelegramBotClient _client,
            long userId,
            int messageId,
            string text,
            InlineKeyboardMarkup markup = null)
        {
            return _client.EditMessageTextAsync(chatId: userId,
                                                messageId: messageId,           
                                                text: GetMarkdownText(text),
                                                parseMode: ParseMode.MarkdownV2,
                                                replyMarkup: markup);
        }
        public static Task<Message> SendMessage(this ITelegramBotClient _client, long userId, MessageData message)
        {
            return _client.SendMessage(userId, message.Text, message.RemoveKeyboard ? new ReplyKeyboardRemove() : message.ReplyMarkup);
        }

        public static async Task SendMessage(this ITelegramBotClient _client, long userId, IEnumerable<MessageData> messages)
        {
            foreach (var message in messages)
            {
                await _client.SendMessage(userId, message);
            }
        }

        public static async Task EditMessages(this ITelegramBotClient _client, long userId, IEnumerable<EditMessageData> messages)
        {
            foreach (var message in messages)
            {
                await _client.EditMessage(userId, message.MessageId, message.Text, message.ReplyMarkup as InlineKeyboardMarkup);
            }
        }

        public static async Task DeleteMessages(this ITelegramBotClient _client, long userId, IEnumerable<int> deleteMessageIds)
        {
            foreach (var messageId in deleteMessageIds)
            {
                await _client.DeleteMessageAsync(userId, messageId);
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
