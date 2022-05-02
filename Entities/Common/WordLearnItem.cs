using Entities.Interfaces;

namespace Entities.Common
{
    public class WordLearnItem : IWord
    {
        public int Id { get; set; }
        public string Eng { get; set; }
        public string Rus { get; set; }
        public WordStatus Status { get; set; }
        public int Recognitions { get; set; }
        public int Order { get; set; }
    }
}
