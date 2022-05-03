using Entities.Common;
using System.Collections.Generic;

namespace Entities.Navigation
{
    public class ActionResult
    {
        public List<MessageData> MessagesToSend { get; set; } = new List<MessageData>();
        public UserState? SwitchToUserState { get; set; }

        public ActionResult Append(ActionResult innerResult)
        {
            var resultMessages = new List<MessageData>();
            resultMessages.AddRange(this.MessagesToSend);
            resultMessages.AddRange(innerResult.MessagesToSend);

            var resultState = innerResult.SwitchToUserState.HasValue
                ? innerResult.SwitchToUserState.Value
                : this.SwitchToUserState;

            return new ActionResult
            {
                MessagesToSend = resultMessages,
                SwitchToUserState = resultState,
            };
        }
    }
}
