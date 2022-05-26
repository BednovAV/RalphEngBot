using DataAccessLayer.Interfaces;
using Entities.Common;
using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
using Entities.Navigation.WordStatistics;
using Helpers;
using LogicLayer.Interfaces.Words;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace LogicLayer.Services.Words
{
    public class WordsAccessor : IWordsAccessor
    {
        private const int WORDS_PAGE_SIZE = 15;

        private readonly IUserWordsDAO _userWordsDAO;
        private readonly IWordsAccessorMessageGenerator _messageGenerator;

        public WordsAccessor(IUserWordsDAO userWordsDAO, IWordsAccessorMessageGenerator messageGenerator)
        {
            _userWordsDAO = userWordsDAO;
            _messageGenerator = messageGenerator;
        }

        public ActionResult ShowUserWords(UserItem user, int pageNumber = 1, bool withAll = false)
        {
            return GetShowUserWordMessages(user, pageNumber, withAll).ToActionResult();
        }

        public ActionResult ShowUserWords(UserItem user, CallbackQuery callbackQuery, SwitchUserWordPageData page)
        {
            return GetShowUserWordMessages(user, page.ToPage, page.WithAll)
                .ToEditMessageData(callbackQuery.Message.MessageId)
                .ToActionResult();
        }

        private MessageData GetShowUserWordMessages(UserItem user, int pageNumber, bool withAll)
        {
            var statisticsData = new WordStatisticsData
            {
                WithAll = withAll,
                WordsLearned = _userWordsDAO.GetUserWordsLearnedCount(user.Id),
            };
            statisticsData.PageData = statisticsData.WithAll
                ? _userWordsDAO.GetAllWordsStatistics(user.Id, pageNumber, WORDS_PAGE_SIZE)
                : _userWordsDAO.GetUserWordsStatistics(user.Id, pageNumber, WORDS_PAGE_SIZE);

            return _messageGenerator.GenerateShowUserWordsMsg(statisticsData);
        }
    }
}
