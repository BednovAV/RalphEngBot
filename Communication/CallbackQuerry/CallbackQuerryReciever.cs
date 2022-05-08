using Entities;
using Entities.Common;
using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
using Helpers;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Grammar;
using LogicLayer.Interfaces.Words;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Communication
{
    public class CallbackQuerryReciever : ICallbackQuerryReciever
    {
        private readonly IWordsLogic _wordsLogic;
        private readonly IWordsAccessor _wordsAccessor;
        private readonly IGrammarTestAccessor _grammarTestAccessor;
        private readonly IGrammarTestLogic _grammarTestLogic;

        public CallbackQuerryReciever(IWordsLogic wordsLogic,
            IWordsAccessor wordsAccessor,
            IGrammarTestAccessor grammarTestAccessor, 
            IGrammarTestLogic grammarTestLogic)
        {
            _wordsLogic = wordsLogic;
            _wordsAccessor = wordsAccessor;
            _grammarTestAccessor = grammarTestAccessor;
            _grammarTestLogic = grammarTestLogic;
        }

        public Dictionary<InlineMarkupType, Func<CallbackQuery, UserItem, string, ActionResult>> CallbackQuerryActionByType
            => new Dictionary<InlineMarkupType, Func<CallbackQuery, UserItem, string, ActionResult>>
            {
                { InlineMarkupType.ExitFromWordsLearning, (callback, user, jsonData) => _wordsLogic.StopLearn(user) },
                { InlineMarkupType.WordHint, (callback, user, jsonData) => _wordsLogic.HintWord(user) },
                { InlineMarkupType.SwitchShowUserWordPage, SwitchShowUserWordPage},
                { InlineMarkupType.BackToLearnGrammarMode, (callback, user, jsonData) => UserState.LearnGrammarMode.ToActionResult()},
                { InlineMarkupType.GoToTheme, GoToTheme},
                { InlineMarkupType.GoToThemeList, (callback, user, jsonData) => _grammarTestAccessor.ShowThemes(user)},
                { InlineMarkupType.StartTest, StartTest},
                { InlineMarkupType.CompleteTest, CompleteTest},
                { InlineMarkupType.GiveAnswer, GiveAnswer},
                { InlineMarkupType.ExitFromTest, (callback, user, jsonData) => _grammarTestAccessor.ShowThemes(user)},

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

        private ActionResult SwitchShowUserWordPage(CallbackQuery callback, UserItem user, string jsonData)
        {
            var data = JsonConvert.DeserializeObject<SwitchUserWordPageData>(jsonData);
            return _wordsAccessor.ShowUserWords(user, callback, data);
        }
        private ActionResult GoToTheme(CallbackQuery callback, UserItem user, string jsonData)
        {
            var data = JsonConvert.DeserializeObject<ThemeData>(jsonData);
            return _grammarTestAccessor.ShowTheme(user, data.ThemeId);
        }
        private ActionResult StartTest(CallbackQuery callback, UserItem user, string jsonData)
        {
            var data = JsonConvert.DeserializeObject<ThemeData>(jsonData);
            return _grammarTestLogic.StartTest(user, data.ThemeId);
        }
        private ActionResult GiveAnswer(CallbackQuery callback, UserItem user, string jsonData)
        {
            var data = JsonConvert.DeserializeObject<GiveAnswerData>(jsonData);
            return _grammarTestLogic.GiveAnswer(callback, user, data);
        }
        private ActionResult CompleteTest(CallbackQuery callback, UserItem user, string jsonData)
        {
            bool? confirm = null;
            if (!string.IsNullOrEmpty(jsonData))
                confirm = JsonConvert.DeserializeObject<bool?>(jsonData);

            return _grammarTestLogic.TryCompleteTest(callback, user, confirm);
        }
    }
}
