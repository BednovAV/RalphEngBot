using Entities.Common;

namespace Entities.Navigation.WordStatistics
{
    public class WordStatisticsData
    {
        public Page<WordStatisticsItem> PageData { get; set; }
        public bool WithAll { get; set; }
        public WordsLearned WordsLearned { get; set; }
    }
}
