using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Common
{
    public class UserItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string TelegramUsername { get; set; }
        public UserState State { get; set; }
    }
}
