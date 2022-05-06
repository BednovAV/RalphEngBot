using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    public class GrammarTest
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CountQuestions { get; set; }

        public List<TestQuestion> TestQuestions { get; set; } = new();
        public List<TheoryLink> TheoryLinks { get; set; } = new();
        public List<UserTest> UserTests { get; set; } = new();
    }
}
