using System;

namespace Entities.Common
{
    [Flags]
    public enum WordStatus
    {
        NotSelected = 1,
        Selected = 2,
        Asked = 4,
        WrongAnswer = 8,
        Learned = 16,
        Hinted = 32,
        InRepetition = 64,
    }
}
