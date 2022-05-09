using Entities.Common.Enums;

namespace Entities.ConfigSections
{
    public class RepetitionWordsConfigSection : IWordsConfigSection
    {
        public static string SectionName => "RepetitionWordsConfiguration";
        public int FirstLevelPoints { get; set; }
        public int SecondLevelPoints { get; set; }
        public int ThirdLevelPoints { get; set; }
        public int MaxWords { get; set; }

        public WordsMode Mode => WordsMode.Repetition;
    }
}
