using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IUserDAO
    {
        void Add(User user);
        void Update(User user);
        void Delete(int id);
        User GetById(long id);
    }
}
