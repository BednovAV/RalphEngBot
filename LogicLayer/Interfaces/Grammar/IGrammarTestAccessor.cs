using Entities.Common;
using Entities.Navigation;
using Telegram.Bot.Types;

namespace LogicLayer.Interfaces.Grammar
{
    public interface IGrammarTestAccessor
    {
        MessageData ShowThemes(UserItem user);
        MessageData ShowTheme(UserItem user, int themeId);
    }
}
