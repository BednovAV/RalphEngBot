using Entities;
using Entities.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy
{
    public interface IStateStrategy
    {
        public IEnumerable<MessageData> Action(Message message, UserItem user);

    }
}
