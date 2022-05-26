using Entities.Common;
using System;
using Telegram.Bot.Types;

namespace Entities.Navigation
{
    public class StateCommand
    {
        public string Key { get; set; }
        public string Description { get; set; }
        public Func<Message, UserItem, ActionResult> Execute { get; set; }
    }
}
