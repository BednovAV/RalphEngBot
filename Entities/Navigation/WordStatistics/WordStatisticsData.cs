using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Navigation.WordStatistics
{
    public class WordStatisticsData
    {
        public Page<WordStatisticsItem> PageData { get; set; }
        public bool WithAll { get; set; }
    }
}
