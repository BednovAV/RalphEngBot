using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.Common.Grammar;
using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
using Helpers;
using LogicLayer.Interfaces.Grammar;
using System;
using System.Linq;
using Telegram.Bot.Types;

namespace LogicLayer.Services.Grammar
{
    public class GrammarTestLogic : IGrammarTestLogic
    {
        private readonly ITestQuestionDAO _testQuestionDAO;
        private readonly IGrammarTestDAO _grammarTestDAO;
        private readonly ITestLogicMessageGenerator _messageGenerator;
        private readonly IGrammarTestAccessor _grammarTestAccessor;

        public GrammarTestLogic(ITestQuestionDAO testQuestionDAO,
            IGrammarTestDAO grammarTestDAO, 
            ITestLogicMessageGenerator messageGenerator,
            IGrammarTestAccessor grammarTestAccessor)
        {
            _testQuestionDAO = testQuestionDAO;
            _grammarTestDAO = grammarTestDAO;
            _messageGenerator = messageGenerator;
            _grammarTestAccessor = grammarTestAccessor;
        }
        public ActionResult ResetTest(CallbackQuery callback, UserItem user, int themeId)
        {
            _grammarTestDAO.RemoveTestResults(user.Id, themeId);
            return _grammarTestAccessor.ShowTheme(user, themeId)
                .ToEditMessageData(callback.Message.MessageId)
                .ToActionResult();
        }
        public ActionResult GiveAnswer(CallbackQuery callback, UserItem user, GiveAnswerData data)
        {
            var updatedQuestion = _testQuestionDAO.GetAndUpdateQuestion(user.Id, data, callback.Message.MessageId);

            return _messageGenerator.GetQuestionMsg(updatedQuestion)
                .ToEditMessageData(callback.Message.MessageId)
                .ToActionResult();
        }

        public ActionResult StartTest(UserItem user, int themeId)
        {
            if (_testQuestionDAO.HasUserQuestion(user.Id))
                return "Пожалуйста, завержите тест".ToActionResult();

            return UserState.GrammarTestInProgress
                .ToActionResult()
                .Append(CreateTest(user, themeId));
        }

        public ActionResult TryCompleteTest(CallbackQuery callback, UserItem user, bool? confirm)
        {
            if (!confirm.HasValue)
                return _messageGenerator.GetCompleteTestConfirmationMsg()
                    .ToEditMessageData(callback.Message.MessageId)
                    .ToActionResult();

            if (confirm.Value)
            {
                return UserState.LearnGrammarMode
                    .ToActionResult(silentSwitch: true)
                    .Append(CompleteTest(callback, user));
            }
            else
            {
                return _messageGenerator.GetCompleteTestsg()
                    .ToEditMessageData(callback.Message.MessageId)
                    .ToActionResult();
            }
        }

        private ActionResult CompleteTest(CallbackQuery callback, UserItem user)
        {
            var testData = _testQuestionDAO.GetUserQuestions(user.Id);
            var testResult = new TestResult
            {
                UserId = user.Id,
                GrammarTestId = testData.TestInfo.Id,
                DateCompleted = DateTime.Now,
                Score = GetTestScore(testData),
            };
            _grammarTestDAO.SaveTestResult(testResult);
            _testQuestionDAO.CleanupUserQuestions(user.Id);

            var result = new ActionResult();
            foreach (var question in testData.QuestionItems)
            {
                if (question.MessageId.HasValue)
                {
                    result.MessagesToEdit.Add(_messageGenerator
                        .GetCompletedQuestion(question)
                        .ToEditMessageData(question.MessageId.Value));
                }
            }
            result.MessagesToEdit.Add(_messageGenerator.GetTestResultMsg(testResult).ToEditMessageData(callback.Message.MessageId));
            return result;
        }

        private int  GetTestScore(InProgressTestData testData)
        {
            var percents = ((double)testData.QuestionItems.Count(q => q.RightAnswer == q.CurrentAnswer) / testData.TestInfo.CountQuestions) * 100;
            return Convert.ToInt32(percents);
        }

        private ActionResult CreateTest(UserItem user, int themeId)
        {
            var testInfo = _grammarTestDAO.GetTestInfo(themeId);
            var questions = _testQuestionDAO.GenerateAndGetTestQuestions(themeId);
            _testQuestionDAO.AddUserQuestions(user.Id, questions);
            return _messageGenerator.GetStartTestMsgs(testInfo, questions).ToActionResult();
        }
    }
}
