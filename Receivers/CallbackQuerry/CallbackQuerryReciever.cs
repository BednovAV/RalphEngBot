using Entities.Common;
using Entities.Navigation;
using LogicLayer.Interfaces;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace Receivers
{
    public class CallbackQuerryReciever : ICallbackQuerryReciever
    {
        private readonly IWordsLogic _wordsLogic;

        public CallbackQuerryReciever(IWordsLogic wordsLogic)
        {
            _wordsLogic = wordsLogic;
        }

        //public Dictionary<InlineMarkupType, Func<Message, UserItem, ActionResult>> CallbackQuerryActionByType
        //    => InitCallbackQuerryActionByType();

        //private Dictionary<InlineMarkupType, Func<Message, UserItem, ActionResult>> InitCallbackQuerryActionByType()
        //{
        //    return new Dictionary<InlineMarkupType, Action<Message, UserItem, ActionResult>>
        //    {
        //        { InlineMarkupType.ExitFromWordsLearning, (msg,) => { } }
        //    }
        //}
        public ActionResult Action(CallbackQuery callbackQuery, UserItem user)
        {
            var callbackItem = JsonConvert.DeserializeObject<CallbackQuerryItem>(callbackQuery.Data);
            switch (callbackItem.Type)
            {
                case InlineMarkupType.ExitFromWordsLearning:
                    return _wordsLogic.StopLearn(user);
                case InlineMarkupType.WordHint:
                    return _wordsLogic.HintWord(user);
            }
            return null;
        }
    }
}
