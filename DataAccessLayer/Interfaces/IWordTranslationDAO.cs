using Entities.Common;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IWordTranslationDAO
    {
        //WordTranslation GetById(int id);
        WordItem GetNewWordForUser(long userId);
        List<WordItem> GetRandomWords(int count);
    }
}
