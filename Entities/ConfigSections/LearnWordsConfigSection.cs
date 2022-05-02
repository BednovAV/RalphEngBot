namespace Entities.ConfigSections
{
    public class LearnWordsConfigSection
    {
        public static string SectionName => "LearnWordsConfiguration";

        public int WordsForLearnCount { get; set; }
        public int RequestWordsCount { get; set; }
        public int FirstLevelPoints { get; set; }
        public int SecondLevelPoints { get; set; }
        public int ThirdLevelPoints { get; set; }

        public int RightAnswersForLearned => FirstLevelPoints + SecondLevelPoints + ThirdLevelPoints;
    }
}
