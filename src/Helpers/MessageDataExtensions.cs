using Entities;
using Entities.Common;
using Entities.Navigation;
using System.Collections.Generic;
using System.Linq;

namespace Helpers
{
    public static class MessageDataExtensions
    {
        public static List<MessageData> Add(this MessageData inner, MessageData outher)
        {
            return new List<MessageData> { inner, outher };
        }
        public static ActionResult ToActionResult(this MessageData messageData, UserState? switchToUserState = null)
        {
            return new ActionResult
            {
                SwitchToUserState = switchToUserState,
                MessagesToSend = new List<MessageData> { messageData }
            };
        }
        public static ActionResult ToActionResult(this EditMessageData editMessageData, UserState? switchToUserState = null)
        {
            return new ActionResult
            {
                SwitchToUserState = switchToUserState,
                MessagesToEdit = new List<EditMessageData> { editMessageData }
            };
        }
        public static ActionResult ToActionResult(this IEnumerable<MessageData> messageData, UserState? switchToUserState = null)
        {
            return new ActionResult
            {
                SwitchToUserState = switchToUserState,
                MessagesToSend = new List<MessageData>(messageData)
            };
        }
        public static ActionResult ToActionResult(this IEnumerable<EditMessageData> editMessageData, UserState? switchToUserState = null)
        {
            return new ActionResult
            {
                SwitchToUserState = switchToUserState,
                MessagesToEdit = new List<EditMessageData>(editMessageData)
            };
        }

        public static EditMessageData ToEditMessageData(this MessageData messageData, int messageId)
        {
            return new EditMessageData
            {
                MessageId = messageId,
                Text = messageData.Text,
                RemoveKeyboard = messageData.RemoveKeyboard,
                ReplyMarkup = messageData.ReplyMarkup,
                ParseMode = messageData.ParseMode,
            };
        }
        public static List<EditMessageData> ToEditMessageData(this IEnumerable<MessageData> messageData, int messageId)
        {
            return messageData.Select(m => m.ToEditMessageData(messageId)).ToList();
        }
    }
}
