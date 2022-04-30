using Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IWordTranslationDAO
    {
        //WordTranslation GetById(int id);
        WordItem GetNewWordForUser(long userId);
    }
}
