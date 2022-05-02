using Entities.Common;
using Entities.ConfigSections;
using Helpers;
using LogicLayer.Interfaces.Words;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace LogicLayer.Services.Words
{
    public class WordsMessageGenerator : IWordsMessageGenerator
    {
        private const string EMOJI_GREEN_CIRCLE = "🟢";
        private const string EMOJI_RED_CIRCLE = "🔴";
        private const string EMOJI_WHITE_CIRCLE = "⚪";
        private const string EMOJI_YELLOW_CIRCLE = "🟡";
        private const string EMOJI_PARTY_POPPER = "🎉";

        private readonly IConfiguration _configuration;

        public LearnWordsConfigSection LearnWordsConfig => _configuration.GetSection(LearnWordsConfigSection.SectionName).Get<LearnWordsConfigSection>();

        public WordsMessageGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MessageData GetNotEnoughWordsMsg(int notEnoughCount)
        {
            return new MessageData { Text = $"Не хватает слов: {notEnoughCount}" };
        }

        public MessageData GetWordSuccesfullySelectedMsg(string word)
        {
            return new MessageData { Text = $"Слово *{word}* успешно добавлено!" };
        }

        public MessageData GetWordNotFoundMsg()
        {
            return new MessageData { Text = "Такого слова нет!" };
        }

        public MessageData GetRightAnswerMsg()
        {
            return new MessageData { Text = "Верно!" };
        }

        public MessageData GetWordLearnedMsg(string word)
        {
            return new MessageData { Text = $"Слово *{word}* выучено! {EMOJI_PARTY_POPPER}" };
        }

        public MessageData GetRequsetNewWordMsg(IEnumerable<string> notSelectedWords)
        {
            return new MessageData { Text = "Выберите слово для изучения", ReplyMarkup = notSelectedWords.GenerateWordsKeyboard() };
        }

        public MessageData GetAskWordMessage(WordLearnItem wordForAsking)
        {
            return new MessageData
            {
                Text = $"Переведите слово на русский: *{wordForAsking.Eng}*\n{CreateWordProgressBar(wordForAsking)}"
            };
        }

        private string CreateWordProgressBar(WordLearnItem word)
        {
            int completed = word.Recognitions;
            int firstLevelRemaining = 0;
            int secondLevelRemaining = 0;
            int thirdLevelRemaining;

            if (completed < LearnWordsConfig.FirstLevelPoints)
            {
                firstLevelRemaining = LearnWordsConfig.FirstLevelPoints - completed;
                secondLevelRemaining = LearnWordsConfig.SecondLevelPoints;
                thirdLevelRemaining = LearnWordsConfig.ThirdLevelPoints;
            }
            else if (completed >= LearnWordsConfig.FirstLevelPoints && completed < LearnWordsConfig.FirstLevelPoints + LearnWordsConfig.SecondLevelPoints)
            {
                secondLevelRemaining = LearnWordsConfig.FirstLevelPoints + LearnWordsConfig.SecondLevelPoints - completed;
                thirdLevelRemaining = LearnWordsConfig.ThirdLevelPoints;
            }
            else
            {
                thirdLevelRemaining = LearnWordsConfig.FirstLevelPoints + LearnWordsConfig.SecondLevelPoints + LearnWordsConfig.ThirdLevelPoints - completed;
            }

            return EMOJI_GREEN_CIRCLE.Repeat(completed)
                 + EMOJI_WHITE_CIRCLE.Repeat(firstLevelRemaining)
                 + EMOJI_YELLOW_CIRCLE.Repeat(secondLevelRemaining)
                 + EMOJI_RED_CIRCLE.Repeat(thirdLevelRemaining);
        }

        public MessageData GetSecondWrongAnswerMsg(WordLearnItem askedWord)
        {
            return new MessageData { Text = $"Вторая ошибка подряд!\nПравильный ответ: *{askedWord.Rus}*\nСчет слова *{askedWord.Eng}* сброшен(" };
        }
        public MessageData GetFirstWrongAnswerMsg()
        {
            return new MessageData { Text = "Ответ неправильный, попробуй еще раз" };
        }
    }
}
