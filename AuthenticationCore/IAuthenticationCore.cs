using Entities;
using Telegram.Bot.Types;

namespace AuthenticationCore
{
    public interface IAuthenticationCore
    {
        Entities.User AuthenticateUser(Chat chat);
    }
}
