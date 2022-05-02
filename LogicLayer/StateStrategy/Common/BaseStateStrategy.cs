using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.Navigation;
using Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy.Common
{
    public abstract class BaseStateStrategy : IStateStrategy
    {
        protected readonly IUserDAO _userDAO;

        protected BaseStateStrategy(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        private List<StateCommand> StateCommands
        {
            get
            {
                var stateCommands = new List<StateCommand>(InitStateCommands()); 
                stateCommands.AddRange(BaseStateCommands);
                return stateCommands;
            }
        }

        protected abstract IEnumerable<StateCommand> InitStateCommands();

        private IEnumerable<StateCommand> BaseStateCommands => new List<StateCommand>
        {
            HelpCommand
        };

        public IEnumerable<MessageData> Action(Message message, UserItem user)
        {
            var messageCommand = message.Text.Split(' ').First();
            var command = StateCommands.FirstOrDefault(c => c.Key == messageCommand);
            if (command != null)
            {
                return command.Execute(message, user);
            }
            return NoCommandAction(message, user);
        }

        protected virtual IEnumerable<MessageData> NoCommandAction(Message message, UserItem user)
        {
            return new MessageData[] { "/help - список доступных команд".ToMessageData() };
        }

        private StateCommand HelpCommand => new StateCommand
        {
            Key = "/help",
            Description = null,
            Execute = (message, user) =>
            {
                var builder = new StringBuilder("Доступные команды:\n");
                foreach (var command in StateCommands)
                {
                    if (command.Description != null)
                    {
                        builder.AppendLine($"{command.Key} - {command.Description}");
                    }
                }

                return new MessageData[] { builder.ToString().ToMessageData() };
            }
        };

        protected StateCommand BackToMainCommand => new StateCommand
        {
            Key = "/back",
            Description = "Выйти",
            Execute = (message, user) =>
            {
                _userDAO.SwitchUserState(user.Id, UserState.WaitingCommand);
                return new MessageData[] { "Вы вернулись в главное меню.\n/help - список доступных команд".ToMessageData() };
            }
        };
    }
}
