namespace Entities.DbModels
{
    public class UserQuestion
    {
        public int Id { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public int TestQuestionId { get; set; }
        public TestQuestion TestQuestion { get; set; }

        public string AnswerOptions { get; set; }
        public string UserAnswer { get; set; }
        public string RightAnswer { get; set; }
        public int Index { get; set; }
        public int? MessageId { get; set; }
    }
}
