using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;
using Entities.Common;
using Entities.Common.Grammar;
using Entities.DbModels;
using Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Services
{
    public class GrammarTestDAO : BaseDAO, IGrammarTestDAO
    {
        public GrammarTestDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public TestInfo GetTestInfo(int themeId)
        {
            return UseContext(db => db.GrammarTests.Find(themeId).Map<TestInfo>());
        }

        public List<ThemeItem> GetThemes()
        {
            return UseContext(db => db.GrammarTests.Map<List<ThemeItem>>());
        }

        public List<UserTestItem> GetUserTests(long userId)
        {
            return UseContext(db => db.UserTests.Where(ut => ut.UserId == userId).Map<List<UserTestItem>>());
        }

        public UserThemeExtendedItem GetUserThemeItem(long userId, int themeId)
        {
            return UseContext(db =>
                {
                    var theme = db.GrammarTests.Include(t => t.TheoryLinks).First(t => t.Id == themeId);
                    var testResult = db.UserTests.FirstOrDefault(ut => ut.UserId == userId && ut.GrammarTestId == themeId);
                    return new UserThemeExtendedItem
                    {
                        Id = theme.Id,
                        Name = theme.Name,
                        TheoryLinks = theme.TheoryLinks.Map<List<LinkItem>>(),
                        DateCompleted = testResult?.DateCompleted,
                        Score = testResult?.Score ?? 0,
                    };
                });
        }

        public void SaveTestResult(TestResult testResult)
        {
            var userTest = testResult.Map<UserTest>();
            UseContext(db =>
            {
                var existingUserTest = db.UserTests.AsNoTracking()
                    .FirstOrDefault(ut => ut.UserId == userTest.UserId && ut.GrammarTestId == userTest.GrammarTestId);
                if (existingUserTest != null)
                {
                    db.UserTests.Update(userTest);
                }
                else
                {
                    db.UserTests.Add(userTest);
                }
            });
        }
    }
}
