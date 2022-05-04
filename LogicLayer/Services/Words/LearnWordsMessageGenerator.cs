using Entities.Common;
using Entities.ConfigSections;
using Entities.Navigation;
using Helpers;
using LogicLayer.Interfaces.Words;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogicLayer.Services.Words
{
    public class LearnWordsMessageGenerator : ILearnWordsMessageGenerator
    {
        private const string EMOJI_GREEN_CIRCLE = "🟢";
        private const string EMOJI_RED_CIRCLE = "🔴";
        private const string EMOJI_WHITE_CIRCLE = "⚪";
        private const string EMOJI_YELLOW_CIRCLE = "🟡";
        private const string EMOJI_PARTY_POPPER = "🎉";

        private readonly IConfiguration _configuration;

        public LearnWordsConfigSection LearnWordsConfig => _configuration.GetSection(LearnWordsConfigSection.SectionName).Get<LearnWordsConfigSection>();

        public LearnWordsMessageGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MessageData GetNotEnoughWordsMsg(int notEnoughCount) => $"Не хватает слов: {notEnoughCount}".ToMessageData();

        public MessageData GetWordSuccesfullySelectedMsg(string word) => $"Слово *{word}* успешно добавлено!".ToMessageData();

        public MessageData GetWordNotFoundMsg() => "Такого слова нет!".ToMessageData();

        public MessageData GetRightAnswerMsg() => "Верно!".ToMessageData();

        public MessageData GetWordLearnedMsg(string word, WordsLearned learnedWords) 
            => $"Слово *{word}({learnedWords.LearnedCount + 1}/{learnedWords.TotalCount})* выучено! {EMOJI_PARTY_POPPER}".ToMessageData();

        public MessageData GetRequsetNewWordMsg(IEnumerable<string> notSelectedWords)
            => new MessageData { Text = "Выберите слово для изучения", ReplyMarkup = notSelectedWords.GenerateWordsKeyboard() };

        public MessageData GetAskWordMsg(WordLearnItem wordForAsking, Language translateFrom, Language translateTo)
            => ($"Переведите слово на {translateTo.GetDescription()}: *{wordForAsking.GetValue(translateFrom)}*" +
                $"\n{CreateWordProgressBar(wordForAsking)}")
                    .ToMessageData(CreateAskWordButtons());

        public MessageData GetSecondWrongAnswerMsg(WordLearnItem askedWord) 
            => $"Вторая ошибка подряд!\nПравильный ответ: *{askedWord.Rus}*\nСчет слова *{askedWord.Eng}* сброшен(".ToMessageData();
        public MessageData GetFirstWrongAnswerMsg() => "Ответ неправильный, попробуй еще раз".ToMessageData();
        public MessageData GetAskWordAnswerOptions(string[] answerOptions) 
            => "Выберите подходящее слово:".ToMessageData(answerOptions.GenerateWordsKeyboard());

        public MessageData GetFirstLevelHint(WordLearnItem askedWord) => $"Правильный ответ: *{askedWord.Rus}*".ToMessageData();
        public MessageData GetAskWordCallMsg() => "Введите слово: ".ToMessageData(removeKeyboard: true);
        private string CreateWordProgressBar(WordLearnItem word)
        {
            int completed = word.Recognitions;
            int firstLevelRemaining = 0;
            int firstLevelPoints = 0;
            int secondLevelRemaining = 0;
            int secondLevelPoints = 0;
            int thirdLevelRemaining = 0;
            int thirdLevelPoints = 0;

            if (completed < LearnWordsConfig.FirstLevelPoints)
            {
                firstLevelRemaining = LearnWordsConfig.FirstLevelPoints - completed;
                firstLevelPoints = completed;

                secondLevelRemaining = LearnWordsConfig.SecondLevelPoints;
                thirdLevelRemaining = LearnWordsConfig.ThirdLevelPoints;
            }
            else if (completed >= LearnWordsConfig.FirstLevelPoints && completed < LearnWordsConfig.FirstLevelPoints + LearnWordsConfig.SecondLevelPoints)
            {
                firstLevelPoints = LearnWordsConfig.FirstLevelPoints;

                secondLevelRemaining = LearnWordsConfig.FirstLevelPoints + LearnWordsConfig.SecondLevelPoints - completed;
                secondLevelPoints = LearnWordsConfig.SecondLevelPoints - secondLevelRemaining;

                thirdLevelRemaining = LearnWordsConfig.ThirdLevelPoints;
            }
            else
            {
                firstLevelPoints = LearnWordsConfig.FirstLevelPoints;
                secondLevelPoints = LearnWordsConfig.SecondLevelPoints;

                thirdLevelRemaining = LearnWordsConfig.FirstLevelPoints + LearnWordsConfig.SecondLevelPoints + LearnWordsConfig.ThirdLevelPoints - completed;
                thirdLevelPoints = LearnWordsConfig.ThirdLevelPoints - thirdLevelRemaining;
            }

            return
                $"С вариантом ответа:           {EMOJI_GREEN_CIRCLE.Repeat(firstLevelPoints)}{EMOJI_YELLOW_CIRCLE.Repeat(firstLevelRemaining)}\n"
                + $"С анлийского на русский:  {EMOJI_GREEN_CIRCLE.Repeat(secondLevelPoints)}{EMOJI_YELLOW_CIRCLE.Repeat(secondLevelRemaining)}\n"
                + $"С русского на английский: {EMOJI_GREEN_CIRCLE.Repeat(thirdLevelPoints)}{EMOJI_YELLOW_CIRCLE.Repeat(thirdLevelRemaining)}";
        }

        private IReplyMarkup CreateAskWordButtons()
        {
            return new InlineKeyboardMarkup(
                new[]
                {
                    new []
                    {
                        InlineMarkupType.WordHint.CreateInlineMarkupItem(),
                        InlineMarkupType.ExitFromWordsLearning.CreateInlineMarkupItem(),
                    },
                });
        }
    }
}
