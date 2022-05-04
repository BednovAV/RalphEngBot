using DataAccessLayer.Interfaces;
using Entities;
using Entities.ConfigSections;
using Entities.Navigation;
using Helpers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Receivers
{
    public class WaitingCommandStrategy : BaseStateStrategy
    {
        public static UserState State => UserState.WaitingCommand;

        private readonly IAdministrationDAO _administrationDAO;
        private readonly IConfiguration _configuration;

        public AdministrationConfigSection AdministrationData
            => _configuration.GetSection(AdministrationConfigSection.SectionName).Get<AdministrationConfigSection>();

        public WaitingCommandStrategy(IUserDAO userDAO, IAdministrationDAO administrationDAO, IConfiguration configuration) : base(userDAO)
        {
            _administrationDAO = administrationDAO;
            _configuration = configuration;
        }

        public override string StateInfo => "*Главное меню*\n" + GetCommandsDescriptions();

        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new StateCommand[]
            {
                RenameCommand,
                LearnWordsCommand,
                ResetDbCommand
            };
        }

        private StateCommand RenameCommand => new StateCommand
        {
            Key = "/rename",
            Description = "Изменить имя",
            Execute = (message, user) => "Как я могу к вам обращаться?".ToActionResult(UserState.WaitingNewName)
        };

        private StateCommand LearnWordsCommand => new StateCommand
        {
            Key = "/learnwords",
            Description = "Изучение слов",
            Execute = (message, user) => UserState.LearnWordsMode.ToActionResult()
        };

        private StateCommand ResetDbCommand => new StateCommand
        {
            Key = "/resetdb",
            Description = null,
            Execute = (message, user) =>
            {
                var enteredPass = message.Text.Split(' ').Skip(1).FirstOrDefault();
                if (enteredPass == AdministrationData.Password)
                {
                    _administrationDAO.ResetDB();
                    return  "Database reset successfully".ToActionResult();
                }
                else
                {
                    return "Invalid password".ToActionResult();
                }
            }
        };
    }
}
