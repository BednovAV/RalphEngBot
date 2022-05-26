using Entities.Common;
using Entities.ConfigSections;
using Entities.Navigation;
using Helpers;
using LogicLayer.Interfaces.Words;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogicLayer.Services.Words
{
    public class WordsLogicMessageGenerator : IWordsLogicMessageGenerator
    {
        private const string EMOJI_GREEN_CIRCLE = "🟢";
        private const string EMOJI_RED_CIRCLE = "🔴";
        private const string EMOJI_WHITE_CIRCLE = "⚪";
        private const string EMOJI_YELLOW_CIRCLE = "🟡";
        private const string EMOJI_PARTY_POPPER = "🎉";
        private const string EMOJI_CONFETTI_BALL = "🎊";
        private const string EMOJI_NOTE = "📝";
        private const string EMOJI_CROSS_MARK = "❌";
        private const string EMOJI_CLOSED_BOOK = "📕";
        private const string EMOJI_ENG_TO_RUS = "🇬🇧-->🇷🇺";
        private const string EMOJI_RUS_TO_ENG = "🇷🇺-->🇬🇧";


        public IWordsConfigSection WordsConfig { get; set; }

        public MessageData GetStartLearnMsg()
        {
            var text = "*Вы перешли в режим изучения слов.*\n" +
                      $"Чтобы слово считалось выученным, вам нужно *{WordsConfig.FirstLevelPoints} раз* выбрать правильный вариант его перевода({EMOJI_NOTE})," +
                      $" *{WordsConfig.SecondLevelPoints} раза* перевести его с английского на русский({EMOJI_ENG_TO_RUS}) " +
                      $"и *{WordsConfig.ThirdLevelPoints} раза* перевести с русского на английский({EMOJI_RUS_TO_ENG})\n" +
                       "*Удачи!*";
            return text.ToMessageData();
        }

        public MessageData GetNotEnoughWordsMsg(int notEnoughCount) => $"Не хватает слов: {notEnoughCount}".ToMessageData();

        public MessageData GetWordSuccesfullySelectedMsg(string word) => $"Слово *{word}* успешно добавлено!".ToMessageData();

        public MessageData GetWordNotFoundMsg() => "Такого слова нет!".ToMessageData();

        public MessageData GetRightAnswerMsg() => "Верно!".ToMessageData();

        public MessageData GetWordLearnedMsg(string word, WordsLearnedCount learnedWords) 
            => $"Слово *{word}({learnedWords.LearnedCount + 1}/{learnedWords.TotalCount})* выучено! {EMOJI_PARTY_POPPER}".ToMessageData();

        public MessageData GetRequsetNewWordMsg(IEnumerable<string> notSelectedWords)
            => new MessageData { Text = "Выберите слово для изучения", ReplyMarkup = notSelectedWords.GenerateWordsKeyboard() };

        public MessageData GetAskWordMsg(WordLearnItem wordForAsking, Language translateFrom, Language translateTo)
            => ($"Переведите слово на {translateTo.GetDescription()}: *{wordForAsking.GetValue(translateFrom)}*" +
                $"\n{CreateWordProgressBar(wordForAsking)}")
                    .ToMessageData(CreateAskWordButtons());

        public MessageData GetSecondWrongAnswerMsg(WordLearnItem askedWord) 
            => $"Вторая ошибка подряд! {EMOJI_CROSS_MARK}\nПравильный ответ: *{askedWord.Rus}*\nСчет слова *{askedWord.Eng}* сброшен".ToMessageData();
        public MessageData GetFirstWrongAnswerMsg() => "Ответ неправильный, попробуй еще раз".ToMessageData();
        public MessageData GetAskWordAnswerOptions(string[] answerOptions) 
            => "Выберите подходящее слово:".ToMessageData(answerOptions.GenerateWordsKeyboard());

        public MessageData GetFirstLevelHint(WordLearnItem askedWord) => $"Правильный ответ: *{askedWord.Rus}*".ToMessageData();
        public MessageData GetAskWordCallMsg() => "Введите слово: ".ToMessageData(removeKeyboard: true);
        public MessageData GetStartRepetitionMsg() => "*Вы перешли в режим повторения слов.*".ToMessageData();
        public MessageData GetWordRepeatedMsg(string word) => $"Слово *{word}* повторено! {EMOJI_CONFETTI_BALL}".ToMessageData();
        public MessageData GetNotRepetitionWordsMsg() => $"Слов для повторения пока что нет {EMOJI_CLOSED_BOOK}".ToMessageData();

        private string CreateWordProgressBar(WordLearnItem word)
        {
            int completed = word.Recognitions;
            int firstLevelRemaining = 0;
            int firstLevelPoints = 0;
            int secondLevelRemaining = 0;
            int secondLevelPoints = 0;
            int thirdLevelRemaining = 0;
            int thirdLevelPoints = 0;

            if (completed < WordsConfig.FirstLevelPoints)
            {
                firstLevelRemaining = WordsConfig.FirstLevelPoints - completed;
                firstLevelPoints = completed;

                secondLevelRemaining = WordsConfig.SecondLevelPoints;
                thirdLevelRemaining = WordsConfig.ThirdLevelPoints;
            }
            else if (completed >= WordsConfig.FirstLevelPoints && completed < WordsConfig.FirstLevelPoints + WordsConfig.SecondLevelPoints)
            {
                firstLevelPoints = WordsConfig.FirstLevelPoints;

                secondLevelRemaining = WordsConfig.FirstLevelPoints + WordsConfig.SecondLevelPoints - completed;
                secondLevelPoints = WordsConfig.SecondLevelPoints - secondLevelRemaining;

                thirdLevelRemaining = WordsConfig.ThirdLevelPoints;
            }
            else
            {
                firstLevelPoints = WordsConfig.FirstLevelPoints;
                secondLevelPoints = WordsConfig.SecondLevelPoints;

                thirdLevelRemaining = WordsConfig.FirstLevelPoints + WordsConfig.SecondLevelPoints + WordsConfig.ThirdLevelPoints - completed;
                thirdLevelPoints = WordsConfig.ThirdLevelPoints - thirdLevelRemaining;
            }

            var result = new List<string>();
            if (WordsConfig.FirstLevelPoints > 0)
            {
                result.Add($"{EMOJI_NOTE}: {EMOJI_GREEN_CIRCLE.Repeat(firstLevelPoints)}{EMOJI_YELLOW_CIRCLE.Repeat(firstLevelRemaining)}");
            }
            if (WordsConfig.SecondLevelPoints > 0)
            {
                result.Add($"{EMOJI_ENG_TO_RUS}: {EMOJI_GREEN_CIRCLE.Repeat(secondLevelPoints)}{EMOJI_YELLOW_CIRCLE.Repeat(secondLevelRemaining)}");
            }
            if (WordsConfig.ThirdLevelPoints > 0)
            {
                result.Add($"{EMOJI_RUS_TO_ENG}: {EMOJI_GREEN_CIRCLE.Repeat(thirdLevelPoints)}{EMOJI_YELLOW_CIRCLE.Repeat(thirdLevelRemaining)}");
            }

            return string.Join('\n', result.ToArray());
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
