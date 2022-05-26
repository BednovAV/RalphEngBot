using Entities;
using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUserDAO
    {
        void Add(UserItem user);
        void Update(UserItem user);
        void Delete(int id);
        UserItem GetById(long id);
        void SwitchUserState(long id, UserState state);
    }
}
