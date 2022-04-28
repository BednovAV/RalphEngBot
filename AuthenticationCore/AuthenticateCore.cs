using DataAccessLayer.Interfaces;
using Telegram.Bot.Types;

namespace AuthenticationCore
{
    public class AuthenticateCore : IAuthenticationCore
    {
        private readonly IUserDAO _userDAO;

        public AuthenticateCore(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public Entities.User AuthenticateUser(Chat chat)
        {
            var user = _userDAO.GetById(chat.Id);
            if (user == null)
            {
                user = new Entities.User
                {
                    Id = chat.Id,
                    Name = chat.Username
                };

                _userDAO.Add(user);
            }

            return user;
        }
    }
}
