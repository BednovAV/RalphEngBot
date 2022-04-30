using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class WordTranslation
    {
        public int Id { get; set; }
        public string Eng { get; set; }
        public string Rus { get; set; }

        public List<User> Users { get; set; } = new();
        public List<UserWord> UserWords { get; set; } = new();
    }
}
