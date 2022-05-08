using Entities.Common;
using Entities.Common.Grammar;
using System.Collections.Generic;

namespace DataAccessLayer.Interfaces
{
    public interface IGrammarTestDAO
    {
        List<ThemeItem> GetThemes();
        List<UserTestItem> GetUserTests(long userId);
        UserThemeExtendedItem GetUserThemeItem(long userId, int themeId);
        TestInfo GetTestInfo(int themeId);
        void SaveTestResult(TestResult testResult);
        void RemoveTestResults(long userId, int themeId);
    }
}
