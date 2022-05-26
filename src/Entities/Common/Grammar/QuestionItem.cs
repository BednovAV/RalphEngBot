using System;
using System.Collections.Generic;

namespace Entities.Common.Grammar
{
    public class QuestionItem
    {
        public int TestQuestionId { get; set; }
        public string Text { get; set; }
        public List<string> AnswerOptions { get; set; }
        public string RightAnswer { get; set; }
        public string CurrentAnswer { get; set; }
        public int Index { get; set; }
        public int? MessageId { get; set; }
        public bool IsSended { get; set; }
        public int CountQuestions { get; set; }
    }
}
