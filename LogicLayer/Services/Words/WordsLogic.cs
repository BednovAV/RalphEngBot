using DataAccessLayer.Interfaces;
using DataAccessLayer.Services;
using Entities;
using Entities.Common;
using Entities.ConfigSections;
using Helpers;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Words;
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
        private const int COUNT_OF_ANSWER_OPTIONS = 4;

        private readonly IUserWordsDAO _userWordsDAO;
        private readonly IWordTranslationDAO _wordTranslationDAO;
        private readonly IConfiguration _configuration;
        private readonly IUserDAO _userDAO;
        private readonly IWordsMessageGenerator _messageGenerator;

        public LearnWordsConfigSection LearnWordsConfig => _configuration.GetSection(LearnWordsConfigSection.SectionName).Get<LearnWordsConfigSection>();

        public WordsLogic(IUserWordsDAO userWordsDAO,
            IConfiguration configuration,
            IWordTranslationDAO wordTranslationDAO,
            IUserDAO userDAO, 
            IWordsMessageGenerator messageGenerator)
        {
            _userWordsDAO = userWordsDAO;
            _configuration = configuration;
            _wordTranslationDAO = wordTranslationDAO;
            _userDAO = userDAO;
            _messageGenerator = messageGenerator;
        }

        public IEnumerable<MessageData> LearnWords(UserItem user)
        {
            var result = new List<MessageData>();

            var notLearnedWords = _userWordsDAO.GetNotLearnedUserWords(user.Id);
            var selectedWords = notLearnedWords.Where(w => w.Status.HasFlag(WordStatus.Selected)).ToList();
            if (selectedWords.Count < LearnWordsConfig.WordsForLearnCount)
            {
                result.Add(_messageGenerator.GetNotEnoughWordsMsg(LearnWordsConfig.WordsForLearnCount - selectedWords.Count));
                result.AddRange(RequestNewWord(user, notLearnedWords));
            }
            else
            {
                result.AddRange(AskWord(user, selectedWords));
            }

            return result;
        }

        public IEnumerable<MessageData> SelectWord(Message message, UserItem user)
        {
            List<MessageData> result = new List<MessageData>();
            if (_userWordsDAO.TrySelectWord(user.Id, message.Text))
            {
                result.Add(_messageGenerator.GetWordSuccesfullySelectedMsg(message.Text));
            }
            else
            {
                result.Add(_messageGenerator.GetWordNotFoundMsg());
            }

            result.AddRange(LearnWords(user));
            return result;
        }

        public IEnumerable<MessageData> ProcessWordResponse(Message message, UserItem user)
        {
            List<MessageData> result = new List<MessageData>();
            var askedWord = _userWordsDAO.GetAskedUserWord(user.Id);
            if (IsCorrectAnswer(message, askedWord))
            {
                result.Add(ProcessRightUserAnswer(askedWord));
            }
            else if (askedWord.Status.HasFlag(WordStatus.WrongAnswer))
            {
                askedWord.Recognitions = 0;
                askedWord.Status ^= WordStatus.Asked | WordStatus.WrongAnswer;
                result.Add(_messageGenerator.GetSecondWrongAnswerMsg(askedWord));
            }
            else
            {
                askedWord.Status |= WordStatus.WrongAnswer;
                _userWordsDAO.UpdateUserWord(user.Id, askedWord);
                result.Add(_messageGenerator.GetFirstWrongAnswerMsg());
                return result;
            }

            _userWordsDAO.UpdateUserWord(user.Id, askedWord);
            result.AddRange(LearnWords(user));
            return result;
        }

        private bool IsCorrectAnswer(Message message, WordLearnItem askedWord)
        {
            var userAnswer = message.Text.Split('/').First().Trim().ToLowerInvariant();
            if (askedWord.Recognitions < LearnWordsConfig.FirstLevelPoints + LearnWordsConfig.SecondLevelPoints)
            {
                var askedWordRuValues = askedWord.Rus
                    .Split('/')
                    .Select(w => w.Trim().ToLowerInvariant())
                    .ToHashSet();
                return askedWordRuValues.Contains(userAnswer);
            }
            else
            {
                return userAnswer.Equals(askedWord.Eng.Trim().ToLowerInvariant());
            }
        }

        private MessageData ProcessRightUserAnswer(WordLearnItem askedWord)
        {
            askedWord.Recognitions++;
            var remainingCount = LearnWordsConfig.RightAnswersForLearned - askedWord.Recognitions;

            MessageData responceMessage;
            if (remainingCount != 0)
            {
                askedWord.Status &= ~(WordStatus.Asked | WordStatus.WrongAnswer);
                responceMessage = _messageGenerator.GetRightAnswerMsg();
            }
            else
            {
                askedWord.Status = WordStatus.Learned;
                responceMessage = _messageGenerator.GetWordLearnedMsg(askedWord.Eng);
            }
            return responceMessage;
        }

        private IEnumerable<MessageData> AskWord(UserItem user, List<WordLearnItem> selectedWords)
        {
            var wordForAsking = selectedWords.RandomItem();
            _userWordsDAO.SetWordIsAsked(user.Id, wordForAsking.Id);
            _userDAO.SwitchUserState(user.Id, UserState.WaitingWordResponse);
            return GetAskWordMessages(wordForAsking);
        }

        private IEnumerable<MessageData> GetAskWordMessages(WordLearnItem wordForAsking)
        {
            var result = new List<MessageData>();
            if (wordForAsking.Recognitions < LearnWordsConfig.FirstLevelPoints + LearnWordsConfig.SecondLevelPoints)
            {
                result.Add(_messageGenerator.GetAskWordMsg(wordForAsking, Language.Eng, Language.Rus));
                if (wordForAsking.Recognitions < LearnWordsConfig.FirstLevelPoints)
                {
                    result.Add(_messageGenerator.GetAskWordAnswerOptions(CreateAnswerOptions(wordForAsking, Language.Rus)));
                }
            }
            else
            {
                result.Add(_messageGenerator.GetAskWordMsg(wordForAsking, Language.Rus, Language.Eng));
            }
            return result;
        }

        private string[] CreateAnswerOptions(WordLearnItem wordForAsking, Language lang)
        {
            var randomWords = _wordTranslationDAO.GetRandomWords(COUNT_OF_ANSWER_OPTIONS - 1);
            randomWords.Add(wordForAsking.Map<WordItem>());
            return randomWords
                .GetShuffled()
                .Select(w => lang is Language.Eng ? w.Eng : w.Rus)
                .ToArray();
        }

        private IEnumerable<MessageData> RequestNewWord(UserItem user, List<WordLearnItem> notLearnedWords)
        {
            var notSelectedWords = GetAndUpdateNotSelectedWords(user, notLearnedWords);
            _userDAO.SwitchUserState(user.Id, UserState.WaitingNewWord);
            return new MessageData[] { _messageGenerator.GetRequsetNewWordMsg(notSelectedWords) };
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
            return notSelectedUserWords.Select(w => w.ToRequestedWord()).ToArray();
        }
    }
}
