using Entities.Common;
using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
using Telegram.Bot.Types;

namespace LogicLayer.Interfaces.Grammar
{
    public interface IGrammarTestLogic
    {
        ActionResult StartTest(UserItem user, int themeId);
        ActionResult TryCompleteTest(CallbackQuery callback, UserItem user, bool? confirm);
        ActionResult GiveAnswer(CallbackQuery callback, UserItem user, GiveAnswerData data);
    }
}
