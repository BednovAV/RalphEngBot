using Entities.Common;
using Entities.Interfaces;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IWordTranslationDAO
    {
        //WordTranslation GetById(int id);
        WordItem GetNewWordForUser(long userId);
        List<WordItem> GetRandomSelectedWords(long userId, int count, params IWord[] excludeWords);
    }
}
