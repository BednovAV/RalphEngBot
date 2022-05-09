using Entities.Common.Enums;

namespace Entities.ConfigSections
{
    public class LearnWordsConfigSection : IWordsConfigSection
    {
        public static string SectionName => "LearnWordsConfiguration";

        public int WordsForLearnCount { get; set; }
        public int RequestWordsCount { get; set; }
        public int FirstLevelPoints { get; set; }
        public int SecondLevelPoints { get; set; }
        public int ThirdLevelPoints { get; set; }
        public int RightAnswersForComplete => FirstLevelPoints + SecondLevelPoints + ThirdLevelPoints;


        public WordsMode Mode => WordsMode.Learning;
    }
}
