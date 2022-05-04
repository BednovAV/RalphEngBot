using Entities.Common;
using Entities.Navigation;
using Entities.Navigation.WordStatistics;

namespace LogicLayer.Interfaces.Words
{
    public interface IWordsAccessorMessageGenerator
    {
        MessageData GenerateShowUserWordsMsg(WordStatisticsData statisticsData);
    }
}
