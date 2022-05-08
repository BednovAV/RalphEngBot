using Entities.Common.Grammar;
using Entities.ConfigSections;
using Microsoft.Extensions.Configuration;

namespace LogicLayer.Services.Grammar.MessageGenerators
{
    public class GrammarTestMessageHelper
    {
        private const string EMOJI_MARK_A = "✅";
        private const string EMOJI_MARK_B = "☑️";
        private const string EMOJI_MARK_C = "❌";

        public static string GetThemeMark(UserThemeItem theme, IConfiguration config)
        {
            if (!theme.DateCompleted.HasValue)
                return string.Empty;
            return GetThemeMark(theme.Score, config);
        }

        public static string GetThemeMark(int score, IConfiguration config)
        {
            var learnGrammarConfig = config.GetSection(LearnGrammarConfigSection.SectionName).Get<LearnGrammarConfigSection>();

            if (score > learnGrammarConfig.MarkA)
                return EMOJI_MARK_A;

            if (score > learnGrammarConfig.MarkB)
                return EMOJI_MARK_B;

            return EMOJI_MARK_C;
        }
    }
}
