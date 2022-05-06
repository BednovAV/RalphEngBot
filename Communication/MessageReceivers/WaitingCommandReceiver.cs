using DataAccessLayer.Interfaces;
using Entities;
using Entities.ConfigSections;
using Entities.Navigation;
using Helpers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Communication
{
    public class WaitingCommandReceiver : BaseMessageReceiver
    {
        public static UserState State => UserState.WaitingCommand;

        private readonly IAdministrationDAO _administrationDAO;
        private readonly IConfiguration _configuration;

        public AdministrationConfigSection AdministrationData
            => _configuration.GetSection(AdministrationConfigSection.SectionName).Get<AdministrationConfigSection>();

        public WaitingCommandReceiver(IAdministrationDAO administrationDAO, IConfiguration configuration)
        {
            _administrationDAO = administrationDAO;
            _configuration = configuration;
        }

        public override string StateInfo => "*Главное меню*\n" + GetCommandsDescriptions();

        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new StateCommand[]
            {
                LearnGrammarCommand,
                LearnWordsCommand,
                ResetDbCommand
            };
        }

        private StateCommand LearnWordsCommand => new StateCommand
        {
            Key = "/learnwords",
            Description = "Изучение слов",
            Execute = (message, user) => UserState.LearnWordsMode.ToActionResult()
        };

        private StateCommand LearnGrammarCommand => new StateCommand
        {
            Key = "/learngrammar",
            Description = "Изучение грамматики",
            Execute = (message, user) => UserState.LearnGrammarMode.ToActionResult()
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
