using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;
using Entities.Common;
using Entities.ConfigSections;
using Entities.DbModels;
using Entities.Navigation;
using Entities.Navigation.WordStatistics;
using Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
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
            .UserWords.Where(w => !w.Status.HasFlag(WordStatus.Learned))
            .Map<IEnumerable<WordLearnItem>>().ToList());
        }

        public List<WordItem> GetNotSelectedUserWords(long userId)
        {
            return UseContext(db => db.Users
            .Find(userId)
            .UserWords.Where(w => w.Status == WordStatus.NotSelected)
            .Map<List<WordItem>>());
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

        public bool TrySelectWord(long userId, string word)
        {
            return UseContext(db =>
            {
                var selectedUserWord = db.Users
                    .Include(u => u.UserWords)
                    .Include(u => u.WordTranslations)
                    .FirstOrDefault(u => u.Id == userId)
                    .UserWords.FirstOrDefault(w => w.Status == WordStatus.NotSelected && w.WordTranslation.ToRequestedWord() == word);

                if (selectedUserWord != null)
                {
                    selectedUserWord.Status = WordStatus.Selected;
                    selectedUserWord.Order = null;
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

        public void SetWordIsAsked(long userId, int wordId)
        {
            UseContext(db =>
                db.Users
                    .Include(u => u.UserWords)
                    .First(u => u.Id == userId)
                    .UserWords.First(w => w.WordTranslationId == wordId)
                    .Status |= WordStatus.Asked);
        }

        public WordLearnItem GetAskedUserWord(long userId)
        {
            return UseContext(db =>
                db.Users
                    .Include(u => u.UserWords)
                    .Include(u => u.WordTranslations)
                    .First(u => u.Id == userId)
                    .UserWords.First(w => w.Status.HasFlag(WordStatus.Asked))
                    .Map<WordLearnItem>());
        }

        public void UpdateUserWord(long userId, WordLearnItem updatedUserWord)
        {
            UseContext(db =>
            {
                var userWord = db.Users
                    .Include(u => u.UserWords)
                    .First(u => u.Id == userId)
                    .UserWords.First(w => w.WordTranslationId == updatedUserWord.Id);

                userWord.Recognitions = updatedUserWord.Recognitions;
                userWord.Status = updatedUserWord.Status;
                userWord.DateLearned = updatedUserWord.DateLearned;
            });
        }

        public void UpdateUserWords(long userId, List<WordLearnItem> updatedUserWords)
        {
            var updatedUserWordsById = updatedUserWords.ToDictionary(u => u.Id);
            UseContext(db =>
                db.Users
                    .Include(u => u.UserWords)
                    .First(u => u.Id == userId)
                    .UserWords.ForEach(userWord =>
                    {
                        if (updatedUserWordsById.TryGetValue(userWord.WordTranslationId, out var updatedUserWord))
                        {
                            userWord.Recognitions = updatedUserWord.Recognitions;
                            userWord.Status = updatedUserWord.Status;
                            userWord.DateLearned = updatedUserWord.DateLearned;
                        }
                    }));
        }

        public void ResetWordStatuses(long userId)
        {
            UseContext(db =>
            {
                db.Users
                    .Include(u => u.UserWords)
                    .First(u => u.Id == userId)
                    .UserWords.Where(w => w.Status.HasFlag(WordStatus.Asked) || w.Status.HasFlag(WordStatus.WrongAnswer))
                    .ForEach(w => w.Status &= ~(WordStatus.Asked | WordStatus.WrongAnswer));
            });
        }

        public Page<WordStatisticsItem> GetUserWordsStatistics(long userId, int pageNumber, int pageSize)
        {
            return UseContext(db =>
               db
               .Users
               .Include(u => u.UserWords)
               .Include(u => u.WordTranslations)
               .First(u => u.Id == userId)
               .UserWords
               .Where(w => !w.Status.HasFlag(WordStatus.NotSelected))
               .OrderBy(w => w.WordTranslation.Eng)
               .Select(w => new WordStatisticsItem
               {
                   Word = w.WordTranslation.Map<WordItem>(),
                   LearnInfo = w.Map<WordLearnInfo>()
               })
               .GetPaged(pageNumber, pageSize));
        }

        public Page<WordStatisticsItem> GetAllWordsStatistics(long userId, int pageNumber, int pageSize)
        {
            return UseContext(db =>
                db
                .WordTranslations
                .Include(w => w.UserWords)
                .OrderBy(w => w.Eng)
                .Select(w => new WordStatisticsItem
                {
                    Word = w.Map<WordItem>(),
                    LearnInfo = w.UserWords.FirstOrDefault(uw => uw.UserId == userId).Map<WordLearnInfo>()
                }).GetPaged(pageNumber, pageSize));
        }

        public WordsLearnedCount GetUserWordsLearnedCount(long userId)
        {
            return UseContext(db => new WordsLearnedCount 
            { 
                TotalCount = db.WordTranslations.Count(),
                LearnedCount = db.Users
                    .Include(w => w.UserWords)
                    .First(u => u.Id == userId)
                    .UserWords
                    .Count(uw => uw.Status.HasFlag(WordStatus.Learned))
            });

        }

        public List<WordLearnItem> GetRepetitionUserWords(long userId)
        {
            return UseContext(db =>
               db
               .Users
               .Include(u => u.UserWords)
               .Include(u => u.WordTranslations)
               .First(u => u.Id == userId)
               .UserWords
               .Where(w => w.Status.HasFlag(WordStatus.InRepetition))
               .Map<List<WordLearnItem>>());
        }

        public List<WordLearnItem> GetOldestLearnedUserWords(long userId, int count, DateTime maxDateLearned)
        {
            return UseContext(db =>
               db
               .Users
               .Include(u => u.UserWords)
               .Include(u => u.WordTranslations)
               .First(u => u.Id == userId)
               .UserWords
               .Where(w => w.Status.HasFlag(WordStatus.Learned) && !w.Status.HasFlag(WordStatus.InRepetition) && w.DateLearned < maxDateLearned)
               .OrderBy(w => w.DateLearned)
               .Take(count)
               .Map<List<WordLearnItem>>());
        }
    }
}
