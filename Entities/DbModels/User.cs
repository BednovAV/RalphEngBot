using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
