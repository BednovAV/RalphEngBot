using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;
using Entities.Common.Grammar;
using Entities.DbModels;
using Entities.Navigation.InlineMarkupData;
using Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Services
{
    public class TestQuestionDAO : BaseDAO, ITestQuestionDAO
    {
        private const string PASS = "__";
        public TestQuestionDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public List<QuestionItem> GenerateAndGetTestQuestions(int themeId)
        {
            return UseContext(db =>
            {
                var index = 1;
                var grammarTest = db.GrammarTests.Include(test => test.TestQuestions).AsNoTracking().First(test => test.Id == themeId);
                var questions = grammarTest.TestQuestions
                    .RandomItems(grammarTest.CountQuestions)
                    .Select(q => 
                    {
                        var answerOptions = JsonConvert.DeserializeObject<List<string>>(q.AnswerOptions);
                        return new QuestionItem
                        {
                            TestQuestionId = q.Id,
                            Index = index++,
                            Text = q.Text,
                            AnswerOptions = answerOptions.GetShuffled(),
                            RightAnswer = answerOptions.First(),
                            CurrentAnswer = answerOptions.First().Contains(",") ? $"{PASS},{PASS}" : PASS,
                        };
                    }).ToList();
                return questions;
            });
        }
        public void AddUserQuestions(long userId, IEnumerable<QuestionItem> questions)
        {
            var questionsForAdd = questions.Select(q => new UserQuestion
            {
                UserId = userId,
                TestQuestionId = q.TestQuestionId,
                AnswerOptions = JsonConvert.SerializeObject(q.AnswerOptions),
                RightAnswer = q.RightAnswer,
                UserAnswer = q.CurrentAnswer,
                Index = q.Index
            }).ToList();

            UseContext(db => questionsForAdd.ForEach(q => db.UserQuestions.Add(q)));
        }

        public QuestionItem GetAndUpdateQuestion(long userId, GiveAnswerData data, int messageId)
        {
            return UseContext(db =>
            {
                var userQuestion = db.UserQuestions.Include(uq => uq.TestQuestion).First(uq => uq.UserId == userId && uq.TestQuestionId == data.QuestionId);
                userQuestion.UserAnswer = data.Value;
                userQuestion.MessageId = messageId;
                return GetQuestionItem(userQuestion);
            });
        }

        public bool HasUserQuestion(long userId)
        {
            return UseContext(db => db.UserQuestions.Any(uq => uq.UserId == userId));
        }

        public InProgressTestData GetUserQuestions(long userId)
        {
            return UseContext(db =>
            {
                var testQuestions = db.UserQuestions
                    .Include(uq => uq.TestQuestion)
                    .Include(uq => uq.TestQuestion.GrammarTest)
                    .Where(uq => uq.UserId == userId);

                return new InProgressTestData
                {
                    TestInfo = testQuestions.First().TestQuestion.GrammarTest.Map<TestInfo>(),
                    QuestionItems = testQuestions.Select(GetQuestionItem).ToList()
                };
            });
        }

        public void CleanupUserQuestions(long userId)
        {
            UseContext(db => db.UserQuestions.RemoveRange(db.UserQuestions.Where(uq => uq.UserId == userId)));
        }

        private QuestionItem GetQuestionItem(UserQuestion userQuestion)
        {
            return new QuestionItem
            {
                TestQuestionId = userQuestion.TestQuestionId,
                Text = userQuestion.TestQuestion.Text,
                AnswerOptions = JsonConvert.DeserializeObject<List<string>>(userQuestion.AnswerOptions),
                CurrentAnswer = userQuestion.UserAnswer,
                RightAnswer = userQuestion.RightAnswer,
                Index = userQuestion.Index,
                MessageId = userQuestion.MessageId,
            };
        }
    }
}
