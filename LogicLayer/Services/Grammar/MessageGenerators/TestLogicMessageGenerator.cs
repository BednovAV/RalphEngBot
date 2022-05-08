using Entities.Common;
using Entities.Common.Grammar;
using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
using Helpers;
using LogicLayer.Interfaces.Grammar;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogicLayer.Services.Grammar.MessageGenerators
{
    public class TestLogicMessageGenerator : ITestLogicMessageGenerator
    {
        private const int MARKUP_WORDS_PER_LINE = 2;
        private const string EMOJI_RIGHT_ANSWER = "✅";
        private const string EMOJI_WRONG_ANSWER = "❌";
        
        private readonly IConfiguration _config;

        public TestLogicMessageGenerator(IConfiguration config)
        {
            _config = config;
        }
        public List<MessageData> GetStartTestMsgs(TestInfo testInfo, List<QuestionItem> questions)
        {
            var result = new List<MessageData>();
            result.Add(testInfo.Description.ToMessageData());
            questions.ForEach(q => result.Add(GetQuestionMsg(q)));
            result.Add(GetCompleteTestsg());
            return result;
        }

        public MessageData GetQuestionMsg(QuestionItem questionItem)
        {
            return ($"{questionItem.Index}. " + string.Format(questionItem.Text, questionItem.CurrentAnswer.Split(',').Select(a => $"*{a}*").ToArray()))
                .ToMessageData(GenerateQuestionMarkup(questionItem));
        }

        public MessageData GetCompleteTestConfirmationMsg()
        {
            return "Вы уверены?".ToMessageData(GenerateConfirmationMarkup());
        }

        public MessageData GetCompleteTestsg()
        {
            return "Выберите ваш вариант ответа вместо пропусков".ToMessageData(GenerateCompleteTestMarkup());
        }

        public MessageData GetCompletedQuestion(QuestionItem question)
        {
            string[] answers;
            if (question.CurrentAnswer == question.RightAnswer)
            {
                answers = question.CurrentAnswer.Split(',').Select(a => $"<b>{a}</b>").ToArray();
            }
            else
            {
                var wrongAnswers = question.CurrentAnswer.Split(',');
                var rightAnswers = question.RightAnswer.Split(',');
                answers = new string[wrongAnswers.Length];
                for (int i = 0; i < wrongAnswers.Length; i++)
                {
                    answers[i] = $"<b><s>{wrongAnswers[i]}</s> {rightAnswers[i]}</b>";
                }
            }
            return $"{question.Index}. {string.Format(question.Text, answers)}"
                .ToMessageData(GenerateCompleteQuestionMarkup(question), parseMode: ParseMode.Html);
        }

        public MessageData GetTestResultMsg(TestResult testResult)
        {
            return $"Ваш результат: {testResult.Score}%{GrammarTestMessageHelper.GetThemeMark(testResult.Score, _config)}"
                .ToMessageData(GenerateTestResultMarkup());
        }

        private IReplyMarkup GenerateTestResultMarkup()
        {
            return new InlineKeyboardMarkup(InlineMarkupType.ExitFromTest.CreateInlineMarkupItem());
        }

        private IReplyMarkup GenerateCompleteQuestionMarkup(QuestionItem question)
        {
            var answers = question.AnswerOptions.Select(answer =>
            {
                if (answer == question.RightAnswer)
                {
                    answer = $"{answer} {EMOJI_RIGHT_ANSWER}";
                }
                else if (answer == question.CurrentAnswer)
                {
                    answer = $"{answer} {EMOJI_WRONG_ANSWER}";
                }
                return InlineKeyboardButton.WithCallbackData(answer);
            });
            return new InlineKeyboardMarkup(answers.Smash(MARKUP_WORDS_PER_LINE));
        }

        private IReplyMarkup GenerateConfirmationMarkup()
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton[]
            {
                InlineMarkupType.CompleteTest.CreateInlineMarkupItem("Да", true),
                InlineMarkupType.CompleteTest.CreateInlineMarkupItem("Нет", false),
            });
        }

        private IReplyMarkup GenerateQuestionMarkup(QuestionItem questionItem)
        {
            var answers = questionItem.AnswerOptions.Select(answer =>
            {
                var data = new GiveAnswerData
                {
                    QuestionId = questionItem.TestQuestionId,
                    Value = answer
                };
                if (answer == questionItem.CurrentAnswer)
                {
                    answer = $"*{answer}*";
                }
                return InlineMarkupType.GiveAnswer.CreateInlineMarkupItem(answer, data);
            });
            return new InlineKeyboardMarkup(answers.Smash(MARKUP_WORDS_PER_LINE));
        }

        private InlineKeyboardMarkup GenerateCompleteTestMarkup()
        {
            return new InlineKeyboardMarkup(InlineMarkupType.CompleteTest.CreateInlineMarkupItem());
        }
    }
}
