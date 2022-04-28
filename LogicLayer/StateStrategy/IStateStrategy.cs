using Entities;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy
{
    public interface IStateStrategy
    {
        public Task Action(Message message, Entities.User user);
    }
}
