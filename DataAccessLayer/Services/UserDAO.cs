using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class UserDAO : BaseDAO, IUserDAO
    {
        public UserDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public void Add(User user)
        {
            UseContext(db => db.Users.Add(user));
        }

        public void Delete(int id)
        {
            UseContext(db => db.Users.Remove(new User { Id = id }));
        }

        public User GetById(long id)
        {
            return UseContext(db => db.Users.Find(id));
        }

        public void Update(User user)
        {
            UseContext(db => db.Users.Update(user));
        }
    }
}
