using Entities.Common;
using Entities.ConfigSections;
using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
using Entities.Navigation.WordStatistics;
using Helpers;
using LogicLayer.Interfaces.Words;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogicLayer.Services.Words
{
    public class WordsAccessorMessageGenerator : IWordsAccessorMessageGenerator
    {
        private const string EMOJI_GREEN_CHECKMARK = "✅";
        private const string EMOJI_REVERSE_BUTTON = "◀️";
        private const string EMOJI_PLAY_BUTTON = "▶️";

        private readonly IConfiguration _configuration;

        public WordsAccessorMessageGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LearnWordsConfigSection LearnWordsConfig => _configuration.GetSection(LearnWordsConfigSection.SectionName).Get<LearnWordsConfigSection>();

        public MessageData GenerateShowUserWordsMsg(WordStatisticsData statisticsData)
        {
            var builder = new StringBuilder();

            builder.AppendLine($"*Выучено слов: {statisticsData.WordsLearned.LearnedCount}/{statisticsData.WordsLearned.TotalCount}*");
            builder.AppendLine("Список " + (statisticsData.WithAll ? string.Empty : "изученных ") + "слов:");

            var index = ((statisticsData.PageData.Number - 1) * statisticsData.PageData.PageSize) + 1;
            foreach (var item in statisticsData.PageData.Data)
            {
                builder.Append($"{index++}. *{item.Word.Eng}* - {item.Word.Rus}");
                if (item.LearnInfo != null && !item.LearnInfo.Status.HasFlag(WordStatus.NotSelected))
                {
                    builder.Append(item.LearnInfo.Status.HasFlag(WordStatus.Learned)
                        ? " " + EMOJI_GREEN_CHECKMARK
                        : $" (в изучении {item.LearnInfo.Recognitions}/{LearnWordsConfig.RightAnswersForLearned})");
                }
                builder.AppendLine();
            }
            builder.AppendLine($"Страница {statisticsData.PageData.Number} из {statisticsData.PageData.TotalPages}.");
            return builder.ToString().ToMessageData(GenerateShowUserWordsMarkup(statisticsData));
        }

        private InlineKeyboardMarkup GenerateShowUserWordsMarkup(WordStatisticsData statisticsData)
        {
            var firstRow = new List<InlineKeyboardButton>();
            if (statisticsData.PageData.Number > 1)
            {
                firstRow.Add(InlineMarkupType.SwitchShowUserWordPage.CreateInlineMarkupItem(EMOJI_REVERSE_BUTTON, new SwitchUserWordPageData
                {
                    ToPage = statisticsData.PageData.Number - 1,
                    WithAll = statisticsData.WithAll
                }));
            }
            if (statisticsData.PageData.Number < statisticsData.PageData.TotalPages)
            {
                firstRow.Add(InlineMarkupType.SwitchShowUserWordPage.CreateInlineMarkupItem(EMOJI_PLAY_BUTTON, new SwitchUserWordPageData
                {
                    ToPage = statisticsData.PageData.Number + 1,
                    WithAll = statisticsData.WithAll
                }));
            }

            var secondRow = new List<InlineKeyboardButton>();
            if (statisticsData.WithAll)
            {
                secondRow.Add(InlineMarkupType.SwitchShowUserWordPage.CreateInlineMarkupItem("Показать мои слова", new SwitchUserWordPageData
                {
                    ToPage = 1,
                    WithAll = false
                }));
            }
            else
            {
                secondRow.Add(InlineMarkupType.SwitchShowUserWordPage.CreateInlineMarkupItem("Показать все слова", new SwitchUserWordPageData
                {
                    ToPage = 1,
                    WithAll = true
                }));
            }

            var thirdRow = new List<InlineKeyboardButton> { InlineMarkupType.ExitFromWordsLearning.CreateInlineMarkupItem("Вернуться в меню") };

            return new InlineKeyboardMarkup(new List<InlineKeyboardButton>[] { firstRow, secondRow, thirdRow });
        }
    }
}
