using Entities.Common.Enums;

namespace Entities.ConfigSections
{
    public interface IWordsConfigSection
    {
        int FirstLevelPoints { get; set; }
        int SecondLevelPoints { get; set; }
        int ThirdLevelPoints { get; set; }
        public int RightAnswersForComplete => FirstLevelPoints + SecondLevelPoints + ThirdLevelPoints;
        public WordsMode Mode { get; }
    }
}
