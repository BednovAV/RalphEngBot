using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    public class TestQuestion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Text { get; set; }
        public string AnswerOptions { get; set; }

        public int GrammarTestId { get; set; }
        public GrammarTest GrammarTest { get; set; }
        public List<UserQuestion> UserQuestions { get; set; } = new();
    }
}