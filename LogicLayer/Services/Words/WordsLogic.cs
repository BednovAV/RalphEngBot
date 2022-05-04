using DataAccessLayer.Interfaces;
using DataAccessLayer.Services;
using Entities;
using Entities.Common;
using Entities.ConfigSections;
using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
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
        private readonly ILearnWordsMessageGenerator _messageGenerator;

        public LearnWordsConfigSection LearnWordsConfig => _configuration.GetSection(LearnWordsConfigSection.SectionName).Get<LearnWordsConfigSection>();

        public WordsLogic(IUserWordsDAO userWordsDAO,
            IConfiguration configuration,
            IWordTranslationDAO wordTranslationDAO,
            IUserDAO userDAO, 
            ILearnWordsMessageGenerator messageGenerator)
        {
            _userWordsDAO = userWordsDAO;
            _configuration = configuration;
            _wordTranslationDAO = wordTranslationDAO;
            _userDAO = userDAO;
            _messageGenerator = messageGenerator;
        }

        public ActionResult StartLearnWords(UserItem user)
        {
            return _messageGenerator.GetStartLearnMsg().ToActionResult().Append(LearnWords(user));
        }

        public ActionResult LearnWords(UserItem user)
        {
            var result = new ActionResult();

            var notLearnedWords = _userWordsDAO.GetNotLearnedUserWords(user.Id);
            var selectedWords = notLearnedWords.Where(w => w.Status.HasFlag(WordStatus.Selected)).ToList();
            if (selectedWords.Count < LearnWordsConfig.WordsForLearnCount)
            {
                result.MessagesToSend.Add(_messageGenerator.GetNotEnoughWordsMsg(LearnWordsConfig.WordsForLearnCount - selectedWords.Count));
                return result.Append(RequestNewWord(user, notLearnedWords));
            }
            else
            {
                return result.Append(AskWord(user, selectedWords));
            }
        }

        public ActionResult StopLearn(UserItem user)
        {
            _userWordsDAO.ResetWordStatuses(user.Id);
            return UserState.LearnWordsMode.ToActionResult();
        }

        public ActionResult SelectWord(Message message, UserItem user)
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

            return result.ToActionResult().Append(LearnWords(user));
        }

        public ActionResult ProcessWordResponse(Message message, UserItem user)
        {
            List<MessageData> resultMsgs = new List<MessageData>();
            var askedWord = _userWordsDAO.GetAskedUserWord(user.Id);
            if (IsCorrectAnswer(message, askedWord))
            {
                resultMsgs.Add(ProcessRightUserAnswer(askedWord, user));
            }
            else if (askedWord.Status.HasFlag(WordStatus.WrongAnswer))
            {
                askedWord.Recognitions = 0;
                askedWord.Status &= ~(WordStatus.Asked | WordStatus.WrongAnswer | WordStatus.Hinted);
                resultMsgs.Add(_messageGenerator.GetSecondWrongAnswerMsg(askedWord));
            }
            else
            {
                askedWord.Status |= WordStatus.WrongAnswer;
                _userWordsDAO.UpdateUserWord(user.Id, askedWord);
                resultMsgs.Add(_messageGenerator.GetFirstWrongAnswerMsg());
                return resultMsgs.ToActionResult();
            }

            _userWordsDAO.UpdateUserWord(user.Id, askedWord);
            return resultMsgs.ToActionResult().Append(LearnWords(user));
        }

        public ActionResult HintWord(UserItem user)
        {
            var askedWord = _userWordsDAO.GetAskedUserWord(user.Id);
            askedWord.Status |= WordStatus.Hinted;
            _userWordsDAO.UpdateUserWord(user.Id, askedWord);

            return GetWordHints(askedWord).ToActionResult();
        }

        private IEnumerable<MessageData> GetWordHints(WordLearnItem askedWord)
        {
            var messages = new List<MessageData>();
            switch (DefineLevel(askedWord.Recognitions))
            {
                case 1:
                    messages.Add(_messageGenerator.GetFirstLevelHint(askedWord));
                    break;
                case 2:
                    messages.Add(_messageGenerator.GetAskWordAnswerOptions(CreateAnswerOptions(askedWord, Language.Rus)));
                    break;
                case 3:
                    messages.Add(_messageGenerator.GetAskWordAnswerOptions(CreateAnswerOptions(askedWord, Language.Eng)));
                    break;
            }
            return messages;
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

        private MessageData ProcessRightUserAnswer(WordLearnItem askedWord, UserItem user)
        {
            if (!askedWord.Status.HasFlag(WordStatus.Hinted))
            {
                askedWord.Recognitions++;
            }
            var remainingCount = LearnWordsConfig.RightAnswersForLearned - askedWord.Recognitions;

            MessageData responceMessage;
            if (remainingCount != 0)
            {
                askedWord.Status &= ~(WordStatus.Asked | WordStatus.WrongAnswer | WordStatus.Hinted);
                responceMessage = _messageGenerator.GetRightAnswerMsg();
            }
            else
            {
                askedWord.Status = WordStatus.Learned;
                var learnedWords = _userWordsDAO.GetUserWordsLearned(user.Id);
                responceMessage = _messageGenerator.GetWordLearnedMsg(askedWord.Eng, learnedWords);
            }
            return responceMessage;
        }

        private ActionResult AskWord(UserItem user, List<WordLearnItem> selectedWords)
        {
            var wordForAsking = selectedWords.RandomItem();
            _userWordsDAO.SetWordIsAsked(user.Id, wordForAsking.Id);
            return GetAskWordMessages(wordForAsking).ToActionResult(UserState.WaitingWordResponse);
        }

        private IEnumerable<MessageData> GetAskWordMessages(WordLearnItem wordForAsking)
        {
            var result = new List<MessageData>();
            switch (DefineLevel(wordForAsking.Recognitions))
            {
                case 1:
                    result.Add(_messageGenerator.GetAskWordMsg(wordForAsking, Language.Eng, Language.Rus));
                    result.Add(_messageGenerator.GetAskWordAnswerOptions(CreateAnswerOptions(wordForAsking, Language.Rus)));
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

        private int DefineLevel(int points)
        {
            if (points < LearnWordsConfig.FirstLevelPoints)
            {
                return 1;
            }
            else if (points < LearnWordsConfig.FirstLevelPoints + LearnWordsConfig.SecondLevelPoints)
            {
                return 2;
            }
            else
            {
                return 3;
            }
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

        private ActionResult RequestNewWord(UserItem user, List<WordLearnItem> notLearnedWords)
        {
            var notSelectedWords = GetAndUpdateNotSelectedWords(user, notLearnedWords);
            return _messageGenerator.GetRequsetNewWordMsg(notSelectedWords).ToActionResult(UserState.WaitingNewWord);
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
