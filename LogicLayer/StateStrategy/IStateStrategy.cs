using Entities;
using Entities.Common;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy
{
    public interface IStateStrategy
    {
        public Task Action(Message message, UserItem user);

    }
}
