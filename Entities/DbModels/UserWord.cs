namespace Entities.DbModels
{
    public class UserWord
    {
        public long UserId { get; set; }
        public User? User { get; set; }

        public int WordTranslationId { get; set; }
        public WordTranslation? WordTranslation { get; set; }

        public bool IsSelected { get; set; }
        public bool IsLearned { get; set; }
        public int Recognitions { get; set; }
        public bool IsAsked { get; set; }
        public int? Order { get; set; }
    }
}
