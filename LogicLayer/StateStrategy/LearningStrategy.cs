using DataAccessLayer.Interfaces;
using Entities;
using Entities.Common;
using Helpers;
using LogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace LogicLayer.StateStrategy
{
    public class LearningStrategy : IStateStrategy
    {
        private readonly IUserDAO _userDAO;
        private readonly IWordsLogic _wordsLogic;
        private readonly ITelegramBotClient _botClient;

        public LearningStrategy(IUserDAO userDAO, IWordsLogic wordsLogic, ITelegramBotClient botClient)
        {
            _userDAO = userDAO;
            _wordsLogic = wordsLogic;
            _botClient = botClient;
        }

        public static UserState State => UserState.LearnWordsMode;

        public Task Action(Message message, UserItem user)
        {
            return message.Text.Split(' ').First() switch
            {
                "/back" => Back(user),
                "/startlearn" => _wordsLogic.LearnWords(user),
                _ => Usage(message)
            };
        }

        public Task<Message> Usage(Message message)
        {
            string usage = "Доступные команды:\n" +
                           "/startlearn - начать изучение слов\n" +
                           "/back - выйти";

            return _botClient.SendMessage(message.Chat.Id, usage);
        }

        private Task Back(UserItem user)
        {
            _userDAO.SwitchUserState(user.Id, UserState.WaitingCommand);
            return _botClient.SendMessage(user.Id, "Режим изучения слов выключен.\n/help - список доступных команд");
        }
    }
}
