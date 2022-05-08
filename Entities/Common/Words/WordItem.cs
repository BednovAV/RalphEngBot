using Entities.Interfaces;

namespace Entities.Common
{
    public class WordItem : IWord
    {
        public int Id { get; set; }
        public string Eng { get; set; }
        public string Rus { get; set; }
    }
}
