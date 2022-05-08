using Entities;
using Entities.Common;
using Entities.Navigation;
using System.Collections.Generic;

namespace Helpers
{
    public static class MessageDataExtensions
    {
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
    }
}
