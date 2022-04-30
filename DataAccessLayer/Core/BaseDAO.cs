using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Core
{
    public abstract class BaseDAO
    {
        protected readonly IConfiguration _configuration;

        private string TypeOfConnection => _configuration.GetSection("DataConfiguration").GetValue<string>("connection");
        private string ConnectionString => _configuration.GetConnectionString(TypeOfConnection);

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
    }
}
