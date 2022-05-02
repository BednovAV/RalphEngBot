using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace Helpers
{
    public static class MarkupGenerator
    {
        private const int WORDS_PER_LINE = 2;

        public static IReplyMarkup GenerateWordsKeyboard(this IEnumerable<string> replyKeyboardData)
        {
            return new ReplyKeyboardMarkup(replyKeyboardData.Select(x => new KeyboardButton(x)).Smash(WORDS_PER_LINE))
            {
                ResizeKeyboard = true,
            };
        }
    }
}
