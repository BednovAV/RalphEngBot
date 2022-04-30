using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Helpers;
using Microsoft.EntityFrameworkCore;
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

        public WordItem GetNewWordForUser(long userId)
        {
            return UseContext(db =>
            {
                var userWordIds = db.Users.Include(u => u.WordTranslations).First(u => u.Id == userId).WordTranslations.Select(x => x.Id).ToHashSet();
                return db.WordTranslations.FirstOrDefault(w => !userWordIds.Contains(w.Id)).Map<WordItem>();
            });
        }

        //public WordTranslation GetById(int id)
        //{
        //    return UseContext(db => db.WordTranslations.Find(id));
        //}
    }
}
