using Entities.Common.Grammar;
using Entities.Navigation.InlineMarkupData;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface ITestQuestionDAO
    {
        void AddUserQuestions(long userId, IEnumerable<QuestionItem> questions);
        List<QuestionItem> GenerateAndGetTestQuestions(int themeId);
        QuestionItem GetAndUpdateQuestion(long userId, GiveAnswerData data, int messageId);
        bool HasUserQuestion(long userId);
        InProgressTestData GetUserQuestions(long userId);
        void CleanupUserQuestions(long userId);
    }
}
