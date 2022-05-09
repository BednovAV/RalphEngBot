using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.ConfigSections;
using Entities.Navigation;
using Helpers;
using LogicLayer.Interfaces;
using LogicLayer.Interfaces.Words;
using LogicLayer.Services.Words;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace LogicLayer.Services
{
    public class LearnWordsLogic : WordsLogic, ILearnWordsLogic
    { 
        public LearnWordsLogic(IUserWordsDAO userWordsDAO, IWordsLogicMessageGenerator messageGenerator, IWordTranslationDAO wordTranslationDAO, IConfiguration configuration) : base(userWordsDAO, messageGenerator, wordTranslationDAO, configuration)
        {
        }

        public LearnWordsConfigSection LearnWordsConfig => _configuration.GetSection(LearnWordsConfigSection.SectionName).Get<LearnWordsConfigSection>();

        public override IWordsConfigSection WordsConfig => LearnWordsConfig;

        public ActionResult StartLearnWords(UserItem user)
        {
            return _messageGenerator.GetStartLearnMsg().ToActionResult().Append(NextWord(user));
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

            return result.ToActionResult().Append(NextWord(user));
        }
        protected override ActionResult NextWord(UserItem user)
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
                return result
                    .Append(UserState.WaitingLearnWordResponse.ToActionResult())
                    .Append(AskWord(user, selectedWords));
            }
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
