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
        private readonly IWordsAccessor _wordsAccessor;
        private readonly ILearnWordsLogic _learnWordsLogic;
        private readonly IGrammarTestAccessor _grammarTestAccessor;
        private readonly IGrammarTestLogic _grammarTestLogic;

        public CallbackQuerryReciever(
            IWordsAccessor wordsAccessor,
            IGrammarTestAccessor grammarTestAccessor,
            IGrammarTestLogic grammarTestLogic, ILearnWordsLogic learnWordsLogic)
        {
            _wordsAccessor = wordsAccessor;
            _grammarTestAccessor = grammarTestAccessor;
            _grammarTestLogic = grammarTestLogic;
            _learnWordsLogic = learnWordsLogic;
        }

        public Dictionary<InlineMarkupType, Func<CallbackQuery, UserItem, string, ActionResult>> CallbackQuerryActionByType
            => new Dictionary<InlineMarkupType, Func<CallbackQuery, UserItem, string, ActionResult>>
            {
                { InlineMarkupType.ExitFromWordsLearning, (callback, user, jsonData) => _learnWordsLogic.StopWordsAction(user) },
                { InlineMarkupType.WordHint, (callback, user, jsonData) => _learnWordsLogic.HintWord(user) },
                { InlineMarkupType.SwitchShowUserWordPage, SwitchShowUserWordPage},
                { InlineMarkupType.BackToLearnGrammarMode, (callback, user, jsonData) => UserState.LearnGrammarMode.ToActionResult()},
                { InlineMarkupType.GoToTheme, GoToTheme},
                { InlineMarkupType.GoToThemeList, GoToThemeList},
                { InlineMarkupType.StartTest, StartTest},
                { InlineMarkupType.CompleteTest, CompleteTest},
                { InlineMarkupType.GiveAnswer, GiveAnswer},
                { InlineMarkupType.ResetTestResult, ResetTestResult},
                { InlineMarkupType.ExitFromTest,  (callback, user, jsonData) => _grammarTestAccessor.ShowThemes(user).ToActionResult()},

            };

        public ActionResult Action(CallbackQuery callbackQuery, UserItem user)
        {
            CallbackQuerryItem callbackItem;
            try
            {
                callbackItem = JsonConvert.DeserializeObject<CallbackQuerryItem>(callbackQuery.Data);
            }
            catch (Exception)
            {
                return ActionResult.GetEmpty();
            }

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
            return _grammarTestAccessor.ShowTheme(user, data.ThemeId)
                .ToEditMessageData(callback.Message.MessageId)
                .ToActionResult();
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
        private ActionResult ResetTestResult(CallbackQuery callback, UserItem user, string jsonData)
        {
            var data = JsonConvert.DeserializeObject<ThemeData>(jsonData);
            return _grammarTestLogic.ResetTest(callback, user, data.ThemeId);
        }
        private ActionResult GoToThemeList(CallbackQuery callback, UserItem user, string jsonData)
        {
            return _grammarTestAccessor.ShowThemes(user)
                .ToEditMessageData(callback.Message.MessageId)
                .ToActionResult();
        }
    }
}
