using Entities.Common;
using Entities.Common.Grammar;
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
        public MessageData GetProgressMsg(List<UserThemeItem> userThemes)
        {
            var builder = new StringBuilder();
            builder.AppendLine("Прогресс по темам:");
            var i = 0;
            foreach (var userTheme in userThemes)
            {
                builder.AppendLine($"\t{i++}. {userTheme.Name}: {GetTestResultForProgress(userTheme)}");
            }

            return builder.ToString().ToMessageData();
        }

        private string GetTestResultForProgress(UserThemeItem userTheme)
        {
            if (userTheme.DateCompleted.HasValue)
            {
                return $"{userTheme.Score}%{GrammarTestMessageHelper.GetThemeMark(userTheme.Score, _config)}";
            }
            else
            {
                return "тест не пройден";
            }
        }

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
            var result = new List<InlineKeyboardButton[]>();
            result.Add(new InlineKeyboardButton[] { InlineMarkupType.StartTest.CreateInlineMarkupItem(new ThemeData { ThemeId = theme.Id }) });
            if (theme.DateCompleted.HasValue)
            {
                result.Add(new InlineKeyboardButton[] { InlineMarkupType.ResetTestResult.CreateInlineMarkupItem(new ThemeData { ThemeId = theme.Id }) });
            }
            result.Add(new InlineKeyboardButton[] { InlineMarkupType.GoToThemeList.CreateInlineMarkupItem() });
            return new InlineKeyboardMarkup(result);
        }
    }
}
