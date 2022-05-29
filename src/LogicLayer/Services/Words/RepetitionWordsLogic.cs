using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.ConfigSections;
using Entities.Navigation;
using Helpers;
using LogicLayer.Interfaces.Words;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace LogicLayer.Services.Words
{
    public class RepetitionWordsLogic : WordsLogic, IRepetitionWordsLogic
    {
        public RepetitionWordsLogic(
            IUserWordsDAO userWordsDAO,
            IWordsLogicMessageGenerator messageGenerator, 
            IWordTranslationDAO wordTranslationDAO,
            IConfiguration configuration)
            : base(userWordsDAO, messageGenerator, wordTranslationDAO, configuration)
        {
        }

        public RepetitionWordsConfigSection RepetitionWordsConfig 
            => _configuration.GetSection(RepetitionWordsConfigSection.SectionName).Get<RepetitionWordsConfigSection>();

        public override IWordsConfigSection WordsConfig => RepetitionWordsConfig;

        public ActionResult StartRepetition(UserItem user)
        {
            return _messageGenerator.GetStartRepetitionMsg().ToActionResult().Append(NextWord(user));
        }

        protected override ActionResult NextWord(UserItem user)
        {
            var userRepetitionWords = _userWordsDAO.GetRepetitionUserWords(user.Id);
            if (userRepetitionWords.Count < RepetitionWordsConfig.MaxWords)
            {
                var dateForRepetition = DateTime.Now.AddDays(-1);
                var learnedUserWords = _userWordsDAO.GetOldestLearnedUserWords(user.Id, RepetitionWordsConfig.MaxWords - userRepetitionWords.Count, dateForRepetition);
                learnedUserWords.ForEach(w =>
                {
                    w.Recognitions = 0;
                    w.Status |= WordStatus.InRepetition;
                    userRepetitionWords.Add(w);
                });
                if (!userRepetitionWords.Any())
                {
                    return _messageGenerator.GetNotRepetitionWordsMsg().ToActionResult(UserState.LearnWordsMode);
                }
                _userWordsDAO.UpdateUserWords(user.Id, learnedUserWords);
            }

            return UserState.WaitingRepetitionWordResponse.ToActionResult()
                .Append(AskWord(user, userRepetitionWords));
        }
    }
}
