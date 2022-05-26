using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.Common.Enums;
using Entities.ConfigSections;
using Entities.Navigation;
using Helpers;
using LogicLayer.Interfaces.Words;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace LogicLayer.Services.Words
{
    public abstract class WordsLogic
    {
        private const int COUNT_OF_ANSWER_OPTIONS = 4;

        protected readonly IConfiguration _configuration;
        protected readonly IUserWordsDAO _userWordsDAO;
        protected readonly IWordsLogicMessageGenerator _messageGenerator;
        protected readonly IWordTranslationDAO _wordTranslationDAO;


        public WordsLogic(IUserWordsDAO userWordsDAO, IWordsLogicMessageGenerator messageGenerator, IWordTranslationDAO wordTranslationDAO, IConfiguration configuration)
        {
            _userWordsDAO = userWordsDAO;
            _messageGenerator = messageGenerator;
            _wordTranslationDAO = wordTranslationDAO;
            _configuration = configuration;
            _messageGenerator.WordsConfig = WordsConfig;
        }
        public LearnWordsConfigSection LearnWordsConfig 
            => _configuration.GetSection(LearnWordsConfigSection.SectionName).Get<LearnWordsConfigSection>();
        public RepetitionWordsConfigSection RepetitionWordsConfig
            => _configuration.GetSection(RepetitionWordsConfigSection.SectionName).Get<RepetitionWordsConfigSection>();

        public abstract IWordsConfigSection WordsConfig { get; }

        public ActionResult StopWordsAction(UserItem user)
        {
            _userWordsDAO.ResetWordStatuses(user.Id);
            return UserState.LearnWordsMode.ToActionResult();
        }

        public ActionResult AskWord(UserItem user, List<WordLearnItem> wordsForAsking)
        {
            var wordForAsking = wordsForAsking.RandomItem();
            _userWordsDAO.SetWordIsAsked(user.Id, wordForAsking.Id);
            return GetAskWordMessages(user, wordForAsking).ToActionResult();
        }

        public ActionResult HintWord(UserItem user)
        {
            var askedWord = _userWordsDAO.GetAskedUserWord(user.Id);
            askedWord.Status |= WordStatus.Hinted;
            _userWordsDAO.UpdateUserWord(user.Id, askedWord);

            return GetWordHints(user, askedWord).ToActionResult();
        }
        public ActionResult ProcessWordResponse(Message message, UserItem user)
        {
            ActionResult result;
            var askedWord = _userWordsDAO.GetAskedUserWord(user.Id);

            if (IsCorrectAnswer(message, askedWord))
            {
                result = ProcessRightUserAnswer(askedWord, user).ToActionResult();
            }
            else if (askedWord.Status.HasFlag(WordStatus.WrongAnswer))
            {
                askedWord.Recognitions = 0;
                askedWord.Status &= ~(WordStatus.Asked | WordStatus.WrongAnswer | WordStatus.Hinted);
                result = _messageGenerator.GetSecondWrongAnswerMsg(askedWord).ToActionResult();
            }
            else
            {
                askedWord.Status |= WordStatus.WrongAnswer;
                _userWordsDAO.UpdateUserWord(user.Id, askedWord);
                return _messageGenerator.GetFirstWrongAnswerMsg().ToActionResult();
            }

            _userWordsDAO.UpdateUserWord(user.Id, askedWord);
            return result.Append(NextWord(user));
        }

        protected abstract ActionResult NextWord(UserItem user);

        private bool IsCorrectAnswer(Message message, WordLearnItem askedWord)
        {
            var userAnswer = message.Text.Split('/').First().Trim().ToLowerInvariant();
            var config = DefineConfig(askedWord);
            if (askedWord.Recognitions < config.FirstLevelPoints + config.SecondLevelPoints)
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

        private MessageData ProcessRightUserAnswer(WordLearnItem askedWord, UserItem user)
        {
            if (!askedWord.Status.HasFlag(WordStatus.Hinted))
            {
                askedWord.Recognitions++;
            }
            var config = DefineConfig(askedWord);
            var remainingCount = config.RightAnswersForComplete - askedWord.Recognitions;

            if (remainingCount != 0)
            {
                askedWord.Status &= ~(WordStatus.Asked | WordStatus.WrongAnswer | WordStatus.Hinted);
                return _messageGenerator.GetRightAnswerMsg();
            }
            else
            {
                askedWord.Status = WordStatus.Learned;
                askedWord.DateLearned = DateTime.Now;
                if (config.Mode == WordsMode.Learning)
                {
                    var learnedWords = _userWordsDAO.GetUserWordsLearnedCount(user.Id);
                    return _messageGenerator.GetWordLearnedMsg(askedWord.Eng, learnedWords);
                }
                else
                {
                    return _messageGenerator.GetWordRepeatedMsg(askedWord.Eng);
                }
            }
        }

        private IEnumerable<MessageData> GetWordHints(UserItem user, WordLearnItem askedWord)
        {
            var messages = new List<MessageData>();
            switch (DefineLevel(askedWord))
            {
                case 1:
                    messages.Add(_messageGenerator.GetFirstLevelHint(askedWord));
                    break;
                case 2:
                    messages.Add(_messageGenerator.GetAskWordAnswerOptions(CreateAnswerOptions(user, askedWord, Language.Rus)));
                    break;
                case 3:
                    messages.Add(_messageGenerator.GetAskWordAnswerOptions(CreateAnswerOptions(user, askedWord, Language.Eng)));
                    break;
            }
            return messages;
        }

        private IEnumerable<MessageData> GetAskWordMessages(UserItem user, WordLearnItem wordForAsking)
        {
            var result = new List<MessageData>();
            switch (DefineLevel(wordForAsking))
            {
                case 1:
                    result.Add(_messageGenerator.GetAskWordMsg(wordForAsking, Language.Eng, Language.Rus));
                    result.Add(_messageGenerator.GetAskWordAnswerOptions(CreateAnswerOptions(user, wordForAsking, Language.Rus)));
                    break;
                case 2:
                    result.Add(_messageGenerator.GetAskWordMsg(wordForAsking, Language.Eng, Language.Rus));
                    result.Add(_messageGenerator.GetAskWordCallMsg());
                    break;
                case 3:
                    result.Add(_messageGenerator.GetAskWordMsg(wordForAsking, Language.Rus, Language.Eng));
                    result.Add(_messageGenerator.GetAskWordCallMsg());
                    break;
                default:
                    throw new NotImplementedException($"Level not defined");
            }

            return result;
        }

        private int DefineLevel(WordLearnItem word)
        {
            var config = DefineConfig(word);
            if (word.Recognitions < config.FirstLevelPoints)
            {
                return 1;
            }
            else if (word.Recognitions < config.FirstLevelPoints + config.SecondLevelPoints)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        private string[] CreateAnswerOptions(UserItem user, WordLearnItem wordForAsking, Language lang)
        {
            var randomWords = DefineConfig(wordForAsking).Mode == WordsMode.Learning 
                ? _wordTranslationDAO.GetRandomSelectedWords(user.Id, COUNT_OF_ANSWER_OPTIONS - 1, wordForAsking)
                : _wordTranslationDAO.GetRandomWords(COUNT_OF_ANSWER_OPTIONS - 1);
            randomWords.Add(wordForAsking.Map<WordItem>());
            return randomWords
                .GetShuffled()
                .Select(w => lang is Language.Eng ? w.Eng : w.Rus)
                .ToArray();
        }
        private IWordsConfigSection DefineConfig(WordLearnItem word)
        {
            return word.Status.HasFlag(WordStatus.InRepetition)
                ? RepetitionWordsConfig
                : LearnWordsConfig;
        }
    }
}
