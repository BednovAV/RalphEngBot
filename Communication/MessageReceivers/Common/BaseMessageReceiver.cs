using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.Navigation;
using Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;

namespace Communication
{
    public abstract class BaseMessageReceiver : IMessageReceiver
    {
        protected readonly IUserDAO _userDAO;

        protected BaseMessageReceiver(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public abstract string StateInfo { get; }


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

        public ActionResult Action(Message message, UserItem user)
        {
            var messageCommand = message.Text.Split(' ').First();
            var command = StateCommands.FirstOrDefault(c => c.Key == messageCommand);
            if (command != null)
            {
                return command.Execute(message, user);
            }
            return NoCommandAction(message, user);
        }

        protected virtual ActionResult NoCommandAction(Message message, UserItem user)
        {
            return GetCommandsDescriptions().ToActionResult();
        }

        private StateCommand HelpCommand => new StateCommand
        {
            Key = "/help",
            Description = null,
            Execute = (message, user) => GetCommandsDescriptions().ToActionResult()
        };

        protected StateCommand BackToMainCommand => new StateCommand
        {
            Key = "/back",
            Description = "Выйти",
            Execute = (message, user) => UserState.WaitingCommand.ToActionResult()
        };


        protected string GetCommandsDescriptions()
        {
            var builder = new StringBuilder("Доступные команды:\n");
            foreach (var command in StateCommands)
            {
                if (command.Description != null)
                {
                    builder.AppendLine($"{command.Key} - {command.Description}");
                }
            }

            return builder.ToString();
        }
    }
}
