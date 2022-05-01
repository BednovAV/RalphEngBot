using DataAccessLayer.Interfaces;
using DataAccessLayer.Services;
using Entities;
using Entities.Common;
using Entities.ConfigSections;
using Helpers;
using LogicLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LogicLayer.Services
{
    public class WordsLogic : IWordsLogic
    {
        private const string EMOJI_GREEN_CIRCLE = "🟢";
        private const string EMOJI_RED_CIRCLE = "🔴";

        private readonly IUserWordsDAO _userWordsDAO;
        private readonly IWordTranslationDAO _wordTranslationDAO;
        private readonly IConfiguration _configuration;
        private readonly IUserDAO _userDAO;
        private readonly ITelegramBotClient _botClient;

        public LearnWordsConfigSection LearnWordsConfig => _configuration.GetSection(LearnWordsConfigSection.SectionName).Get<LearnWordsConfigSection>();

        public WordsLogic(IUserWordsDAO userWordsDAO,
            IConfiguration configuration,
            IWordTranslationDAO wordTranslationDAO,
            IUserDAO userDAO, ITelegramBotClient botClient)
        {
            _userWordsDAO = userWordsDAO;
            _configuration = configuration;
            _wordTranslationDAO = wordTranslationDAO;
            _userDAO = userDAO;
            _botClient = botClient;
        }

        public async Task<Message> LearnWords(UserItem user)
        {
            var notLearnedWords = _userWordsDAO.GetNotLearnedUserWords(user.Id);
            var selectedWords = notLearnedWords.Where(w => w.Status.HasFlag(WordStatus.Selected)).ToList();
            if (selectedWords.Count < LearnWordsConfig.WordsForLearnCount)
            {
                await _botClient.SendMessage(user.Id, $"Не хватает слов: {LearnWordsConfig.WordsForLearnCount - selectedWords.Count}");
                return await RequestNewWord(user, notLearnedWords);
            }

            return await AskWord(user, selectedWords);
        }

        public async Task<Message> SelectWord(Message message, UserItem user)
        {
            if (_userWordsDAO.SelectWord(user.Id, message.Text))
            {
                await _botClient.SendMessage(user.Id, $"Слово \"{message.Text}\" успешно добавлено!");
            }
            else
            {
                await _botClient.SendMessage(user.Id, "Такого слова нет!");
            }

            return await LearnWords(user);
        }

        public async Task<Message> ProcessWordResponse(Message message, UserItem user)
        {
            var userAnswer = message.Text.Trim().ToLowerInvariant();
            var askedWord = _userWordsDAO.GetAskedUserWord(user.Id);
            var askedWordRuValues = askedWord.Rus.Split('/')
                .Select(w => w.Trim().ToLowerInvariant())
                .ToHashSet();

            if (askedWordRuValues.Contains(userAnswer))
            {
                await ProcessRightUserAnswer(user, askedWord);
            }
            else if (askedWord.Status.HasFlag(WordStatus.WrongAnswer))
            {
                askedWord.Recognitions = 0;
                askedWord.Status ^= WordStatus.Asked | WordStatus.WrongAnswer;
                await _botClient.SendMessage(user.Id, $"Вторая ошибка подряд!\nПравильный ответ: {askedWord.Rus}\nСчет слова *{askedWord.Eng}* сброшен(");
            }
            else
            {
                askedWord.Status |= WordStatus.WrongAnswer;
                _userWordsDAO.UpdateUserWord(user.Id, askedWord);
                return await _botClient.SendMessage(user.Id, $"Ответ неправильный, попробуй еще раз");
            }
            _userWordsDAO.UpdateUserWord(user.Id, askedWord);
            return await LearnWords(user);
        }

        private Task<Message> ProcessRightUserAnswer(UserItem user, WordLearnItem askedWord)
        {
            askedWord.Recognitions++;
            var remainingCount = LearnWordsConfig.RightAnswersForLearned - askedWord.Recognitions;
            var replyText = new StringBuilder("Верно!");
            if (remainingCount != 0)
            {
                askedWord.Status &= ~(WordStatus.Asked | WordStatus.WrongAnswer);
            }
            else
            {
                askedWord.Status = WordStatus.Learned;
                replyText.Append($"\nСлово *{askedWord.Eng}* выучено!");
            }
            return _botClient.SendMessage(user.Id, replyText.ToString());
        }

        private Task<Message> AskWord(UserItem user, List<WordLearnItem> selectedWords)
        {
            var wordForAsking = selectedWords.RandomItem();
            _userWordsDAO.SetWordIsAsked(user.Id, wordForAsking.Id);
            _userDAO.SwitchUserState(user.Id, UserState.WaitingWordResponse);
            return _botClient.SendMessage(user.Id, $"Переведите слово на русский: *{wordForAsking.Eng}*\n" +
                                                   $"Прогресс: {CreateWordProgressBar(wordForAsking)}");
        }

        private Task<Message> RequestNewWord(UserItem user, List<WordLearnItem> notLearnedWords)
        {
            var notSelectedWords = GetAndUpdateNotSelectedWords(user, notLearnedWords);
            _userDAO.SwitchUserState(user.Id, UserState.WaitingNewWord);
            return _botClient.SendMessage(user.Id, "Выберите слово для изучения", notSelectedWords.GenerateWordsKeyboard());
        }

        private string[] GetAndUpdateNotSelectedWords(UserItem user, List<WordLearnItem> notLearnedWords)
        {
            var notSelectedUserWords = new WordLearnItem[LearnWordsConfig.RequestWordsCount];
            notLearnedWords.Where(w => w.Status == WordStatus.NotSelected).ToList().ForEach(w => notSelectedUserWords[w.Order] = w);
            for (int i = 0; i < LearnWordsConfig.RequestWordsCount; i++)
            {
                if (notSelectedUserWords[i] == null)
                {
                    notSelectedUserWords[i] = _wordTranslationDAO.GetNewWordForUser(user.Id).Map<WordLearnItem>();
                    notSelectedUserWords[i].Order = i;
                    _userWordsDAO.AddUserWord(user.Id, notSelectedUserWords[i].Id, notSelectedUserWords[i].Order);
                }
            }
            return notSelectedUserWords.Select(w => w.Eng).ToArray();
        }

        private string CreateWordProgressBar(WordLearnItem wordForAsking)
        {
            return EMOJI_GREEN_CIRCLE.Repeat(wordForAsking.Recognitions) 
                 + EMOJI_RED_CIRCLE.Repeat(LearnWordsConfig.RightAnswersForLearned - wordForAsking.Recognitions);
        }
    }
}
