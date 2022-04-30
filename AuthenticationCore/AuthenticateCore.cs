using DataAccessLayer.Interfaces;
using Entities.Common;
using Telegram.Bot.Types;

namespace AuthenticationCore
{
    public class AuthenticateCore : IAuthenticationCore
    {
        private readonly IUserDAO _userDAO;
        private readonly IUserWordsDAO _userWordsDAO;

        public AuthenticateCore(IUserDAO userDAO, IUserWordsDAO userWordsDAO)
        {
            _userDAO = userDAO;
            _userWordsDAO = userWordsDAO;
        }

        public UserItem AuthenticateUser(Chat chat)
        {
            var id = chat.Id;
            var user = _userDAO.GetById(id);
            if (user == null)
            {
                user = new UserItem
                {
                    Id = id,
                    Name = chat.Username
                };

                _userDAO.Add(user);
                _userWordsDAO.InitWordsForUser(id);
            }

            return user;
        }
    }
}
