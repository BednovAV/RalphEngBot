using System;

namespace Entities.Common.Grammar
{
    public class TestResult
    {
        public long UserId { get; set; }
        public int GrammarTestId { get; set; }
        public int Score { get; set; }
        public DateTime? DateCompleted { get; set; }
    }
}
