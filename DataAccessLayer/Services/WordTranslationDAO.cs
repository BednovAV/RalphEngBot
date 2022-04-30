using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Helpers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Services
{
    public class WordTranslationDAO : BaseDAO, IWordTranslationDAO
    {
        public WordTranslationDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public List<WordItem> GetNewWordsForUser(long userId, int count)
        {
            return UseContext(db =>
            {
                var userWordIds = db.Users.Find(userId).WordTranslations.Select(x => x.Id).ToHashSet();
                return db.WordTranslations.Where(w => !userWordIds.Contains(w.Id)).Take(count).Map<List<WordItem>>();
            });
        }

        //public WordTranslation GetById(int id)
        //{
        //    return UseContext(db => db.WordTranslations.Find(id));
        //}
    }
}
