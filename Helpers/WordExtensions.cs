using Entities.Interfaces;

namespace Helpers
{
    public static class WordExtensions
    {
        public static string ToRequestedWord(this IWord word)
        {
            return $"{word.Eng} - {word.Rus}";
        }
    }
}
