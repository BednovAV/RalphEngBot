using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;
using Entities.Common;
using Entities.ConfigSections;
using Helpers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Services
{
    public class UserWordsDAO : BaseDAO, IUserWordsDAO
    {
        public LearnWordsConfigSection LearnWordsConfig => _configuration.GetSection(LearnWordsConfigSection.SectionName).Get<LearnWordsConfigSection>();

        public UserWordsDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public List<WordLearnItem> GetLearnItemsByUser(long userId)
        {
            return UseContext(db => db.Users.Find(userId).UserWords.Map<List<WordLearnItem>>());
        }

        public List<WordLearnItem> GetNotLearnedUserWords(long userId)
        {
            return UseContext(db => db.Users.Find(userId).UserWords.Where(w => !w.IsLearned).Map<List<WordLearnItem>>());
        }

        public List<WordItem> GetNotSelectedUserWords(long userId)
        {
            return UseContext(db => db.Users.Find(userId).UserWords.Where(w => !w.IsSelected).Map<List<WordItem>>());
        }

        public void InitWordsForUser(long id)
        {
            UseContext(db =>
            {
                var user = db.Users.Find(id);
                if (user.UserWords.Any()) return;

                var words = db.WordTranslations.Take(LearnWordsConfig.RequestWordsCount);
                user.WordTranslations.AddRange(words);
            });
        }
    }
}
