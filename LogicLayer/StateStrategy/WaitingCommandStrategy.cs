using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Entities.ConfigSections;
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
        private readonly ITelegramBotClient _botClient;
        private readonly IWordsLogic _wordsLogic;
        private readonly IConfiguration _configuration;
        private readonly IAdministrationDAO _administrationDAO;

        public AdministrationConfigSection AdministrationData 
            => _configuration.GetSection(AdministrationConfigSection.SectionName).Get<AdministrationConfigSection>();

        public WaitingCommandStrategy(IUserDAO userDAO,
            ITelegramBotClient botClient,
            IWordsLogic wordsLogic,
            IConfiguration configuration,
            IAdministrationDAO administrationDAO)
        {
            _userDAO = userDAO;
            _botClient = botClient;
            _wordsLogic = wordsLogic;
            _configuration = configuration;
            _administrationDAO = administrationDAO;
        }

        public static UserState State => UserState.WaitingCommand;

        public Task Action(Message message, UserItem user)
        {
            return message.Text.Split(' ').First() switch
            {
                "/rename" => RenameUser(message, user),
                "/learnwords" => StartLearnWords(message, user),
                "/resetdb" => ResetDB(message),
                _ => Usage(message, user)
            };
        }

        private Task<Message> ResetDB(Message message)
        {
            var responceText = string.Empty;

            var enteredPass = message.Text.Split(' ').Skip(1).FirstOrDefault();
            if (enteredPass == AdministrationData.Password)
            {
                _administrationDAO.ResetDB();
                responceText = "Database reset successfully";
            }
            else
            {
                responceText = "Invalid password";
            }

            return _botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: responceText,
                                                        replyMarkup: new ReplyKeyboardRemove());
        }

        private Task<Message> StartLearnWords(Message message, UserItem user)
        {
            return _wordsLogic.LearnWords(user);
        }

        private Task<Message> RenameUser(Message message, UserItem user)
        {
            user.State = UserState.WaitingNewName;
            _userDAO.Update(user);

            return _botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                   text: "How can I contact you?");
        }

        public Task<Message> Usage(Message message, UserItem user)
        {
            string usage = $"Hello, {user.Name}.\n" +
                           $"I can:\n" +
                            "/rename - change call\n" +
                            "/learnwords - Learn english words\n" +
                            "/resetdb {password} - Reset the database";

            return _botClient.SendTextMessageAsync(chatId: message.Chat.Id,
                                                        text: usage,
                                                        replyMarkup: new ReplyKeyboardRemove());
        }
    }
}
