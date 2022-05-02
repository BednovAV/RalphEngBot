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

        public MessageData GetNotEnoughWordsMsg(int notEnoughCount) => $"Не хватает слов: {notEnoughCount}".ToMessageData();

        public MessageData GetWordSuccesfullySelectedMsg(string word) => $"Слово *{word}* успешно добавлено!".ToMessageData();

        public MessageData GetWordNotFoundMsg() => "Такого слова нет!".ToMessageData();

        public MessageData GetRightAnswerMsg() => "Верно!".ToMessageData();

        public MessageData GetWordLearnedMsg(string word) => $"Слово *{word}* выучено! {EMOJI_PARTY_POPPER}".ToMessageData();

        public MessageData GetRequsetNewWordMsg(IEnumerable<string> notSelectedWords)
            => new MessageData { Text = "Выберите слово для изучения", ReplyMarkup = notSelectedWords.GenerateWordsKeyboard() };

        public MessageData GetAskWordMessage(WordLearnItem wordForAsking)
            => $"Переведите слово на русский: *{wordForAsking.Eng}*\n{CreateWordProgressBar(wordForAsking)}".ToMessageData();

        public MessageData GetSecondWrongAnswerMsg(WordLearnItem askedWord) 
            => $"Вторая ошибка подряд!\nПравильный ответ: *{askedWord.Rus}*\nСчет слова *{askedWord.Eng}* сброшен(".ToMessageData();
        public MessageData GetFirstWrongAnswerMsg() => "Ответ неправильный, попробуй еще раз".ToMessageData();

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
    }
}
