using Entities;
using Entities.Common;
using LogicLayer.Interfaces;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy
{
    public class WaitingNewWordStrategy : IStateStrategy
    {
        private IWordsLogic _wordsLogic;

        public WaitingNewWordStrategy(IWordsLogic wordsLogic)
        {
            _wordsLogic = wordsLogic;
        }

        public static UserState State => UserState.WaitingNewWord;

        public Task Action(Message message, UserItem user)
        {
            return _wordsLogic.SelectWord(message, user);
        }
    }
}
