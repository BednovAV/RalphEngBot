using Entities;
using Entities.Common;
using Telegram.Bot.Types;

namespace AuthenticationCore
{
    public interface IAuthenticationCore
    {
        UserItem AuthenticateUser(Chat chat);
    }
}
