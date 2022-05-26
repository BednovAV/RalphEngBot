using Entities.Common;
using Entities.Interfaces;

namespace Helpers
{
    public static class WordExtensions
    {
        public static string ToRequestedWord(this IWord word)
        {
            return $"{word.Eng} - {word.Rus}";
        }

        public static string GetValue(this IWord word, Language lang)
            => lang switch
            {
                Language.Rus => word.Rus,
                Language.Eng => word.Eng,
                _ => string.Empty
            };
    }
}
