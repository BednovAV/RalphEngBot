using Entities.Common;
using Entities.Navigation;
using Entities.Navigation.WordStatistics;
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
        void InitWordsForUser(long userId);
        bool TrySelectWord(long userId, string word);
        void AddUserWord(long userId, int wordId, int order);
        void SetWordIsAsked(long userId, int wordId);
        WordLearnItem GetAskedUserWord(long userId);
        void UpdateUserWord(long userId, WordLearnItem updatedUserWord);
        void ResetWordStatuses(long userId);
        Page<WordStatisticsItem> GetUserWordsStatistics(long userId, int pageNumber, int pageSize);
        Page<WordStatisticsItem> GetAllWordsStatistics(long userId, int pageNumber, int pageSize);
    }
}
