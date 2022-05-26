using Entities.Common;

namespace Entities.Navigation.WordStatistics
{
    public class WordStatisticsData
    {
        public Page<WordStatisticsItem> PageData { get; set; }
        public bool WithAll { get; set; }
        public WordsLearnedCount WordsLearned { get; set; }
    }
}
