using Entities.Common;
using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Words;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Receivers
{
    public class CallbackQuerryReciever : ICallbackQuerryReciever
    {
        private readonly IWordsLogic _wordsLogic;
        private readonly IWordsAccessor _wordsAccessor;

        public CallbackQuerryReciever(IWordsLogic wordsLogic, IWordsAccessor wordsAccessor)
        {
            _wordsLogic = wordsLogic;
            _wordsAccessor = wordsAccessor;
        }

        public Dictionary<InlineMarkupType, Func<CallbackQuery, UserItem, string, ActionResult>> CallbackQuerryActionByType
            => new Dictionary<InlineMarkupType, Func<CallbackQuery, UserItem, string, ActionResult>>
            {
                { InlineMarkupType.ExitFromWordsLearning, (callback, user, jsonData) => _wordsLogic.StopLearn(user) },
                { InlineMarkupType.WordHint, (callback, user, jsonData) => _wordsLogic.HintWord(user) },
                { InlineMarkupType.SwitchShowUserWordPage, (callback, user, jsonData) => 
                {
                    var data = JsonConvert.DeserializeObject<SwitchUserWordPageData>(jsonData);
                    return _wordsAccessor.ShowUserWords(user, callback, data);
                }},
            };

        public ActionResult Action(CallbackQuery callbackQuery, UserItem user)
        {
            var callbackItem = JsonConvert.DeserializeObject<CallbackQuerryItem>(callbackQuery.Data);
            if (CallbackQuerryActionByType.TryGetValue(callbackItem.Type, out var action))
            {
                return action(callbackQuery, user, callbackItem.Data);
            }
            return ActionResult.GetEmpty();
        }
    }
}
