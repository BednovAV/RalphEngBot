using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ConfigSections
{
    public class DataConfigurationConfigSection
    {
        public static string SectionName => "DataConfiguration";

        public string Connection { get; set; }
        public string ScriptsPath { get; set; }
    }
}
