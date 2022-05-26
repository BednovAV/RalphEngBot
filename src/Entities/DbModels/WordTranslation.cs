using Entities.DbModels;
using Entities.Interfaces;
using System.Collections.Generic;

namespace Entities
{
    public class WordTranslation : IWord
    {
        public int Id { get; set; }
        public string Eng { get; set; }
        public string Rus { get; set; }

        public List<User> Users { get; set; } = new();
        public List<UserWord> UserWords { get; set; } = new();
    }
}
