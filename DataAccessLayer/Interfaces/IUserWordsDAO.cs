using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUserWordsDAO
    {
        List<WordLearnItem> GetLearnItemsByUser(long userId);
        List<WordLearnItem> GetNotLearnedUserWords(long userId);
        List<WordItem> GetNotSelectedUserWords(long userId);
        void InitWordsForUser(long id);
        bool SelectWord(long userId, string text);
        void AddUserWord(long userId, int wordId, int order);
    }
}
