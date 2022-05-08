using Entities.Common;
using Entities.Common.Grammar;
using Entities.Navigation;
using System.Collections.Generic;

namespace LogicLayer.Interfaces.Grammar
{
    public interface ITestLogicMessageGenerator
    {
        List<MessageData> GetStartTestMsgs(TestInfo testInfo, List<QuestionItem> questions);
        MessageData GetQuestionMsg(QuestionItem questionItem);
        MessageData GetCompleteTestConfirmationMsg();
        MessageData GetCompleteTestsg();
        MessageData GetCompletedQuestion(QuestionItem question);
        MessageData GetTestResultMsg(TestResult testResult);
    }
}
