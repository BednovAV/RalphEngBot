using DataAccessLayer.Interfaces;
using DataAccessLayer.Services;
using Entities.Common;
using Entities.ConfigSections;
using LogicLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace LogicLayer.Services
{
    public class WordsLogic : IWordsLogic
    {
        private IUserWordsDAO _userWordsDAO;
        private IWordTranslationDAO _wordTranslationDAO;
        private IConfiguration _configuration;

        public LearnWordsConfigSection LearnWordsConfig => _configuration.GetSection(LearnWordsConfigSection.SectionName).Get<LearnWordsConfigSection>();

        public WordsLogic(IUserWordsDAO userWordsDAO, IConfiguration configuration, IWordTranslationDAO wordTranslationDAO)
        {
            _userWordsDAO = userWordsDAO;
            _configuration = configuration;
            _wordTranslationDAO = wordTranslationDAO;
        }

        public Task<Message> LearnWords(UserItem user)
        {
            var notLearnedWords = _userWordsDAO.GetNotLearnedUserWords(user.Id);
            if (notLearnedWords.Count < LearnWordsConfig.WordsForLearnCount)
            {
                return RequestNewWord(user, notLearnedWords);
            }

            return AskWord(user, notLearnedWords);
        }

        private Task<Message> AskWord(UserItem user, List<WordLearnItem> notLearnedWords)
        {
            throw new NotImplementedException();
        }

        private Task<Message> RequestNewWord(UserItem user, List<WordLearnItem> notLearnedWords)
        {
            var notSelectedWords = GetNotSelectedWords(user, notLearnedWords);

            throw new NotImplementedException();
        }

        private List<WordLearnItem> GetNotSelectedWords(UserItem user, List<WordLearnItem> notLearnedWords)
        {
            var notSelectedUserWords = notLearnedWords.Where(w => !w.IsSelected).ToList();
            if (notLearnedWords.Count < LearnWordsConfig.RequestWordsCount)
            {
                var newWords = _wordTranslationDAO.GetNewWordsForUser(user.Id, LearnWordsConfig.RequestWordsCount - notLearnedWords.Count)
                    .Select(w => new WordLearnItem 
                    {
                        Id = w.Id,
                        Eng = w.Eng,
                        Rus = w.Rus,
                        IsSelected = false,
                        IsLearned = false,
                        Recognitions = 0
                    }).ToList();
                notSelectedUserWords.AddRange(newWords);
            }
            return notSelectedUserWords;
        }
    }
}
