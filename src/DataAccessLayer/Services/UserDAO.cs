using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Helpers;
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

        public void Add(UserItem user)
        {
            var userDbItem = user.Map<User>();
            UseContext(db => db.Users.Add(userDbItem));
        }

        public void Delete(int id)
        {
            UseContext(db => db.Users.Remove(new User { Id = id }));
        }

        public UserItem GetById(long id)
        {
            return UseContext(db => db.Users.Find(id).Map<UserItem>());
        }

        public void SwitchUserState(long id, UserState state)
        {
            UseContext(db =>
            {
                var user = db.Users.Find(id);
                user.State = state;
                db.Users.Update(user);
            });
        }

        public void Update(UserItem user)
        {
            var userDbItem = user.Map<User>();
            UseContext(db => db.Users.Update(userDbItem));
        }
    }
}
