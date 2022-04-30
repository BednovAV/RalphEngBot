namespace Entities.ConfigSections
{
    public class LearnWordsConfigSection
    {
        public static string SectionName => "LearnWordsConfiguration";

        public int WordsForLearnCount { get; set; }
        public int RequestWordsCount { get; set; }
    }
}
