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
        public static ActionResult ToActionResult(this IEnumerable<MessageData> messageData, UserState? switchToUserState = null)
        {
            return new ActionResult
            {
                SwitchToUserState = switchToUserState,
                MessagesToSend = new List<MessageData>(messageData)
            };
        }
    }
}
