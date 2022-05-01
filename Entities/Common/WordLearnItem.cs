using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Common
{
    public class WordLearnItem
    {
        public int Id { get; set; }
        public string Eng { get; set; }
        public string Rus { get; set; }
        public WordStatus Status { get; set; }
        public int Recognitions { get; set; }
        public int Order { get; set; }
    }
}
