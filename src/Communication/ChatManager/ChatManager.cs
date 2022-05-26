using Entities.Common;
using Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Communication
{
    public class ChatManager : IChatManager
    {
        private readonly ITelegramBotClient _botClient;

        public ChatManager(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        #region Send

        public async Task<Message> SendMessage(long userId, MessageData message)
        {
            return await SendMessageInner(userId, message.Text, message.ParseMode, message.RemoveKeyboard ? new ReplyKeyboardRemove() : message.ReplyMarkup);
        }

        public async Task SendMessages(long userId, IEnumerable<MessageData> messages)
        {
            await messages.ForEachAsync(async msg => await SendMessage(userId, msg));
        }

        #endregion Send

        #region Edit

        public Task<Message> EditMessage(long userId, EditMessageData message)
        {
            return EditMessageInner(userId, message.MessageId, message.Text, message.ParseMode, message.RemoveKeyboard 
                ? new InlineKeyboardMarkup(Enumerable.Empty<InlineKeyboardButton>())
                : message.ReplyMarkup as InlineKeyboardMarkup);
        }

        public async Task EditMessages(long userId, IEnumerable<EditMessageData> messages)
        {
            await messages.ForEachAsync(async msg => await EditMessage(userId, msg));
        }

        #endregion Edit

        #region Delete

        public async Task DeleteMessages(long userId, IEnumerable<int> deleteMessageIds)
        {
            await deleteMessageIds.ForEachAsync(async msgId => await _botClient.DeleteMessageAsync(userId, msgId));
        }

        #endregion Delete

        private Task<Message> SendMessageInner(long userId,
           string text,
           ParseMode parseMode,
           IReplyMarkup replyMarkup = null)
        {
            return _botClient.SendTextMessageAsync(chatId: userId,
                                                text: text,//GetMarkdownText(text),
                                                parseMode: parseMode,
                                                replyMarkup: replyMarkup);
        }
        private Task<Message> EditMessageInner(long userId,
            int messageId,
            string text,
            ParseMode parseMode,
            InlineKeyboardMarkup markup = null)
        {
            return _botClient.EditMessageTextAsync(chatId: userId,
                                                messageId: messageId,
                                                text: text,//GetMarkdownText(text),
                                                parseMode: parseMode,
                                                replyMarkup: markup);
        }

        private static string GetMarkdownV2Text(string text)
        {
            var textBuilder = new StringBuilder(text);
            for (int i = 0; i < textBuilder.Length; i++)
            {
                if (textBuilder[i] is '!' or '-' or '.' or '<' or '>')// or '(' or ')')
                {
                    textBuilder.Insert(i, '\\');
                    i++;
                }
            }

            return textBuilder.ToString();
        }
    }
}
