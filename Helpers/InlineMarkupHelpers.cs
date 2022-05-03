using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace Helpers
{
    public static class InlineMarkupHelpers
    {

        public static InlineKeyboardButton CreateInlineMarkupItem(this InlineMarkupType type)
        {
            return new CallbackQuerryItem { Type = type }.CreateInlineMarkupItemInner();
        }

        private static InlineKeyboardButton CreateInlineMarkupItemInner(this CallbackQuerryItem item)
        {
            return InlineKeyboardButton.WithCallbackData(item.Type.GetDescription(), JsonConvert.SerializeObject(item));
        }
    }
}
