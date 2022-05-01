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
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LogicLayer.Services
{
    public class WordsLogic : IWordsLogic
    {
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
            var countOfSelected = notLearnedWords.Count(w => w.IsSelected);
            if (countOfSelected < LearnWordsConfig.WordsForLearnCount)
            {
                await _botClient.SendMessage(user.Id, $"Нужно еще {LearnWordsConfig.WordsForLearnCount - countOfSelected} слов");
                return await RequestNewWord(user, notLearnedWords);
            }

            return await AskWord(user, notLearnedWords);
        }

        public async Task<Message> SelectWord(Message message, UserItem user)
        {
            if (!_userWordsDAO.SelectWord(user.Id, message.Text))
            {
                await _botClient.SendMessage(user.Id, "Такого слова нет!");
            }
            else
            {
                await _botClient.SendMessage(user.Id, $"Слово \"{message.Text}\" успешно добавлено!");
            }
            return await LearnWords(user);
        }

        private Task<Message> AskWord(UserItem user, List<WordLearnItem> notLearnedWords)
        {
            throw new NotImplementedException();
        }

        private Task<Message> RequestNewWord(UserItem user, List<WordLearnItem> notLearnedWords)
        {
            var notSelectedWords = GetAndUpdateNotSelectedWords(user, notLearnedWords);
            _userDAO.SwitchUserState(user.Id, UserState.WaitingNewWord);
            return _botClient.SendMessage(user.Id, "Выберете слово для изучения", notSelectedWords);
        }

        private string[] GetAndUpdateNotSelectedWords(UserItem user, List<WordLearnItem> notLearnedWords)
        {
            var notSelectedUserWords = new WordLearnItem[LearnWordsConfig.RequestWordsCount];
            notLearnedWords.Where(w => !w.IsSelected).ToList().ForEach(w => notSelectedUserWords[w.Order] = w);
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
    }
}
