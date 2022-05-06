namespace Entities.DbModels
{
    public class UserQuestion
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int TestQuestionId { get; set; }
        public TestQuestion TestQuestion { get; set; }

        public string Choice { get; set; }
    }
}
