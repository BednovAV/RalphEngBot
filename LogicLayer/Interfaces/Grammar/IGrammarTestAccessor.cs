using Entities.Common;
using Entities.Navigation;

namespace LogicLayer.Interfaces.Grammar
{
    public interface IGrammarTestAccessor
    {
        ActionResult ShowThemes(UserItem user);
        ActionResult ShowTheme(UserItem user, int themeId);
    }
}
