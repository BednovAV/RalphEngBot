using DataAccessLayer.Interfaces;
using Entities.Common;
using Entities.Common.Grammar;
using Entities.Navigation;
using Helpers;
using LogicLayer.Interfaces.Grammar;
using System.Collections.Generic;
using System.Linq;

namespace LogicLayer.Services.Grammar
{
    public class GrammarTestAccessor : IGrammarTestAccessor
    {
        private readonly IGrammarTestDAO _grammarTestDAO;
        private readonly ITestAccessorMessageGenerator _messageGenerator;

        public GrammarTestAccessor(IGrammarTestDAO grammarTestDAO, ITestAccessorMessageGenerator messageGenerator)
        {
            _grammarTestDAO = grammarTestDAO;
            _messageGenerator = messageGenerator;
        }

        public ActionResult ShowTheme(UserItem user, int themeId)
        {
            var userThemeItem = _grammarTestDAO.GetUserThemeItem(user.Id, themeId);
            return _messageGenerator.GetShowThemeMsg(userThemeItem).ToActionResult();
        }

        public ActionResult ShowThemes(UserItem user)
        {
            var userThemes = GetUserThemes(_grammarTestDAO.GetThemes(), _grammarTestDAO.GetUserTests(user.Id));
            return _messageGenerator.GetThemesListMsg(userThemes).ToActionResult();
        }

        private List<UserThemeItem> GetUserThemes(List<ThemeItem> allThemes, List<UserTestItem> userTests)
        {
            var userTestsByTestId = userTests.ToDictionary(ut => ut.GrammarTestId);
            return allThemes.Select(theme => 
            {
                var userThemeItem = theme.Map<UserThemeItem>();
                if (userTestsByTestId.TryGetValue(theme.Id, out var userTest))
                {
                    userThemeItem.Score = userTest.Score;
                    userThemeItem.DateCompleted = userTest.DateCompleted;
                }
                return userThemeItem;
            }).ToList();
        }
    }
}
