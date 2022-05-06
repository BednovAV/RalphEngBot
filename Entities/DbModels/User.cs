using Entities.DbModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string TelegramUsername { get; set; }
        public UserState State { get; set; }

        public List<WordTranslation> WordTranslations { get; set; } = new();
        public List<UserWord> UserWords { get; set; } = new();
        public List<UserTest> UserTests { get; set; } = new();
        public List<UserQuestion> UserQuestions { get; set; } = new();
    }
}
