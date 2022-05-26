using System;

namespace Entities.DbModels
{
    public class UserTest
    {
        public long UserId { get; set; }
        public User User { get; set; }

        public int GrammarTestId { get; set; }
        public GrammarTest GrammarTest { get; set; }

        public int Score { get; set; }
        public DateTime? DateCompleted { get; set; }
    }
}
