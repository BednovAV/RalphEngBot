using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public enum UserState
    {
        WaitingCommand = 0,
        WaitingNewName = 1,
        WaitingNewWord = 2,
        WaitingWordResponse = 3,
    }
}
