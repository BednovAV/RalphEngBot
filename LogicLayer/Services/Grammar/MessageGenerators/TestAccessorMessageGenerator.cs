using Entities.Common;
using Entities.Common.Grammar;
using Entities.ConfigSections;
using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
using Helpers;
using LogicLayer.Interfaces.Grammar;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogicLayer.Services.Grammar.MessageGenerators
{
    public class TestAccessorMessageGenerator : ITestAccessorMessageGenerator
    {
        private readonly IConfiguration _config;

        public TestAccessorMessageGenerator(IConfiguration config)
        {
            _config = config;
        }

        public MessageData GetShowThemeMsg(UserThemeExtendedItem userThemeItem)
        {


            var builder = new StringBuilder();

            builder.AppendLine($"Тема: {userThemeItem.Name}");
            if (userThemeItem.DateCompleted.HasValue)
            {
                builder.AppendLine($"Завершен: {userThemeItem.DateCompleted.Value.ToString("dd.MM.yyyy")}");
                builder.AppendLine($"Оценка: {userThemeItem.Score}%{GrammarTestMessageHelper.GetThemeMark(userThemeItem.Score, _config)}");
            }

            builder.AppendLine($"Теория:");
            var index = 1;
            foreach (var link in userThemeItem.TheoryLinks)
            {
                builder.AppendLine($"\t[{index++}. {link.Name}]({link.Url})");
            }
            return builder.ToString().ToMessageData(GenerateThemeMarkup(userThemeItem));
        }

        public MessageData GetThemesListMsg(List<UserThemeItem> userThemes)
            => "Список доступных тем:".ToMessageData(GenerateThemesListMarkup(userThemes));

        private IReplyMarkup GenerateThemesListMarkup(List<UserThemeItem> userThemes)
        {
            var resultRows = new List<List<InlineKeyboardButton>>();
            var index = 1;
            foreach (var theme in userThemes)
            {
                var row = new List<InlineKeyboardButton>();
                var dataItem = new ThemeData { ThemeId = theme.Id };
                row.Add(InlineMarkupType.GoToTheme
                    .CreateInlineMarkupItem($"{index++}. {theme.Name} {GrammarTestMessageHelper.GetThemeMark(theme, _config)}", dataItem));
                resultRows.Add(row);
            }
            resultRows.Add(new List<InlineKeyboardButton> { InlineMarkupType.BackToLearnGrammarMode.CreateInlineMarkupItem() });
            return new InlineKeyboardMarkup(resultRows);
        }

        private IReplyMarkup GenerateThemeMarkup(UserThemeExtendedItem theme)
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new InlineKeyboardButton[] { InlineMarkupType.StartTest.CreateInlineMarkupItem(new ThemeData { ThemeId = theme.Id }) },
                new InlineKeyboardButton[] { InlineMarkupType.GoToThemeList.CreateInlineMarkupItem() }
            });
        }
    }
}
