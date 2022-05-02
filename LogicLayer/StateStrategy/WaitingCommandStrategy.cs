using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.ConfigSections;
using Helpers;
using LogicLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace LogicLayer.StateStrategy
{
    public class WaitingCommandStrategy : IStateStrategy
    {
        private readonly IUserDAO _userDAO;
        private readonly IConfiguration _configuration;
        private readonly IAdministrationDAO _administrationDAO;

        public AdministrationConfigSection AdministrationData 
            => _configuration.GetSection(AdministrationConfigSection.SectionName).Get<AdministrationConfigSection>();

        public WaitingCommandStrategy(IUserDAO userDAO,
            IConfiguration configuration,
            IAdministrationDAO administrationDAO)
        {
            _userDAO = userDAO;
            _configuration = configuration;
            _administrationDAO = administrationDAO;
        }

        public static UserState State => UserState.WaitingCommand;

        public IEnumerable<MessageData> Action(Message message, UserItem user)
        {
            return message.Text.Split(' ').First() switch
            {
                "/rename" => RenameUser(message, user),
                "/learnwords" => SwitchToLearnWordsMode(user),
                "/resetdb" => ResetDB(message),
                _ => Usage(message, user)
            };
        }

        private IEnumerable<MessageData> ResetDB(Message message)
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

        private IEnumerable<MessageData> SwitchToLearnWordsMode(UserItem user)
        {
            _userDAO.SwitchUserState(user.Id, UserState.LearnWordsMode);
            return new MessageData[] { "Режим изучения слов включен.\n/help - список доступных команд".ToMessageData() };
        }

        private IEnumerable<MessageData> RenameUser(Message message, UserItem user)
        {
            _userDAO.SwitchUserState(user.Id, UserState.WaitingNewName);
            return new MessageData[] { "Как я могу к вам обращаться?".ToMessageData() };
        }

        public IEnumerable<MessageData> Usage(Message message, UserItem user)
        {
            string usage = $"Hello, {user.Name}.\n" +
                           $"I can:\n" +
                            "/rename - change call\n" +
                            "/learnwords - Learn english words\n" +
                            "/resetdb {password} - Reset the database";

            return new MessageData[] { usage.ToMessageData() };
        }
    }
}
