using System.Collections.Generic;

namespace Entities.DbModels
{
    public class TestQuestion
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string AnswerOptions { get; set; }

        public int GrammarTestId { get; set; }
        public GrammarTest GrammarTest { get; set; }
        public List<UserQuestion> UserQuestions { get; set; } = new();
    }
}