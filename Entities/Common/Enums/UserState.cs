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
        LearnGrammarMode = 1,
        WaitingNewWord = 2,
        WaitingWordResponse = 3,
        LearnWordsMode = 4,
        GrammarTestInProgress = 5,
    }
}
