using Entities.Common;
using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
using Telegram.Bot.Types;

namespace LogicLayer.Interfaces.Words
{
    public interface IWordsAccessor
    {
        ActionResult ShowUserWords(UserItem user, int pageNumber = 1, bool withAll = false);
        ActionResult ShowUserWords(UserItem user, CallbackQuery callbackQuery, SwitchUserWordPageData page);
    }
}
