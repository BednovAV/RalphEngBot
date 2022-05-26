using Entities.Navigation;
using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace Helpers
{
    public static class InlineMarkupHelpers
    {
        public static InlineKeyboardButton CreateInlineMarkupItem(this InlineMarkupType type, string text) 
            => new CallbackQuerryItem { Type = type }.CreateInlineMarkupItemInner(text);

        public static InlineKeyboardButton CreateInlineMarkupItem<T>(this InlineMarkupType type, string text, T data)
           => new CallbackQuerryItem { Type = type, Data = JsonConvert.SerializeObject(data) }.CreateInlineMarkupItemInner(text);

        public static InlineKeyboardButton CreateInlineMarkupItem(this InlineMarkupType type) 
            => type.CreateInlineMarkupItem(type.GetDescription());

        public static InlineKeyboardButton CreateInlineMarkupItem<T>(this InlineMarkupType type, T data) 
            => type.CreateInlineMarkupItem(type.GetDescription(), data);

        private static InlineKeyboardButton CreateInlineMarkupItemInner(this CallbackQuerryItem item, string text)
        {
            return InlineKeyboardButton.WithCallbackData(text, JsonConvert.SerializeObject(item));
        }
    }
}
