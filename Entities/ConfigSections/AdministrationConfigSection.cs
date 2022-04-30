using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ConfigSections
{
    public class AdministrationConfigSection
    {
        public static string SectionName => "Administration";

        public string Password { get; set; }
    }
}
