using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;
using Entities.Common;
using Entities.ConfigSections;
using Entities.DbModels;
using Helpers;
using Microsoft.EntityFrameworkCore;
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
            return UseContext(db => db.Users
            .Include(x => x.UserWords)
            .Include(x => x.WordTranslations)
            .First(x => x.Id == userId)
            .UserWords.Where(w => !w.IsLearned)
            .Map<IEnumerable<WordLearnItem>>().ToList());
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

                var words = db.WordTranslations.Take(LearnWordsConfig.RequestWordsCount).Select(w => new UserWord
                {
                    WordTranslation = w,
                }).ToList();
                var orderIndex = 0;
                foreach (var word in words)
                {
                    word.Order = orderIndex++;
                }
                user.UserWords.AddRange(words);
            });
        }

        public bool SelectWord(long id, string engText)
        {
            return UseContext(db =>
            {
                var selectedUserWord = db.Users
                    .Include(u => u.UserWords)
                    .Include(u => u.WordTranslations)
                    .FirstOrDefault(u => u.Id == id)
                    .UserWords.FirstOrDefault(w => !w.IsSelected && w.WordTranslation.Eng == engText);

                if (selectedUserWord != null)
                {
                    selectedUserWord.IsSelected = true;
                    return true;
                }
                return false;
            });
        }

        public void AddUserWord(long userId, int wordId, int order)
        {
            UseContext(db =>
            {
                db.Users
                    .Find(userId)
                    .UserWords.Add(new UserWord
                    {
                        UserId = userId,
                        WordTranslationId = wordId,
                        Order = order
                    });
            });

        }
    }
}
