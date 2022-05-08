using System;

namespace Entities.Common.Grammar
{
    public class UserTestItem
    {
        public int GrammarTestId { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public DateTime? DateCompleted { get; set; }
    }
}
