using DataAccessLayer.Core;
using DataAccessLayer.Interfaces;
using Microsoft.Extensions.Configuration;

namespace DataAccessLayer.Services
{
    public class AdministrationDAO : BaseDAO, IAdministrationDAO
    {
        public AdministrationDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public void ResetDB()
        {
            UseContext(db =>
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            });
        }
    }
}
