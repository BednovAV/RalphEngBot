using Entities.Common;
using System.Collections.Generic;

namespace Entities.Navigation
{
    public class ActionResult
    {
        public List<MessageData> MessagesToSend { get; set; } = new List<MessageData>();
        public UserState? SwitchToUserState { get; set; }
        public List<int> MessageIdsToDelete { get; set; } = new List<int>();
        public List<EditMessageData> MessagesToEdit { get; set; } = new List<EditMessageData>();

        public ActionResult Append(ActionResult innerResult)
        {
            var resultMessages = new List<MessageData>();
            resultMessages.AddRange(this.MessagesToSend);
            resultMessages.AddRange(innerResult.MessagesToSend);

            var resultEditMessages = new List<EditMessageData>();
            resultEditMessages.AddRange(this.MessagesToEdit);
            resultEditMessages.AddRange(innerResult.MessagesToEdit);

            var resultState = innerResult.SwitchToUserState.HasValue
                ? innerResult.SwitchToUserState.Value
                : this.SwitchToUserState;

            var resultToDeleteMessages = new List<int>();
            resultToDeleteMessages.AddRange(this.MessageIdsToDelete);
            resultToDeleteMessages.AddRange(innerResult.MessageIdsToDelete);

            return new ActionResult
            {
                MessagesToSend = resultMessages,
                SwitchToUserState = resultState,
                MessageIdsToDelete = resultToDeleteMessages,
                MessagesToEdit = resultEditMessages
            };
        }

        public static ActionResult GetEmpty() => new ActionResult();
    }
}
