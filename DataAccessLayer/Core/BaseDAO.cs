using Entities.ConfigSections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Core
{
    public abstract class BaseDAO
    {
        protected readonly IConfiguration _configuration;

        protected DataConfigurationConfigSection DataConfiguration
            => _configuration.GetSection(DataConfigurationConfigSection.SectionName).Get<DataConfigurationConfigSection>();
        private string ConnectionString => _configuration.GetConnectionString(DataConfiguration.Connection);

        protected BaseDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected void UseContext(Action<ApplicationContext> action)
        {
            using (var db = new ApplicationContext(ConnectionString))
            {
                action(db);
                db.SaveChanges();
            }
        }

        protected T UseContext<T>(Func<ApplicationContext, T> function)
        {
            using (var db = new ApplicationContext(ConnectionString))
            {
                var result = function(db);
                db.SaveChanges();
                return result;
            }
        }

        protected void ExecuteSqlNonQuery(string sql)
        {
            using (var context = new ApplicationContext(ConnectionString))
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    context.Database.OpenConnection();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
