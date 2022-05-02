using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.ConfigSections;
using Entities.Navigation;
using Helpers;
using LogicLayer.StateStrategy.Common;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy
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
            Execute = (message, user) =>
            {
                _userDAO.SwitchUserState(user.Id, UserState.WaitingNewName);
                return new MessageData[] { "Как я могу к вам обращаться?".ToMessageData() };
            }
        };

        private StateCommand LearnWordsCommand => new StateCommand
        {
            Key = "/learnwords",
            Description = "Режим изучения слов",
            Execute = (message, user) =>
            {
                _userDAO.SwitchUserState(user.Id, UserState.LearnWordsMode);
                return new MessageData[] { "Режим изучения слов включен.\n/help - список доступных команд".ToMessageData() };
            }
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
                    return new MessageData[] { "Database reset successfully".ToMessageData() };
                }
                else
                {
                    return new MessageData[] { "Invalid password".ToMessageData() };
                }
            }
        };
    }
}
