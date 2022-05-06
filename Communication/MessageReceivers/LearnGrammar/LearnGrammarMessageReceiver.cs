using DataAccessLayer.Interfaces;
using Entities;
using Entities.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public class LearnGrammarMessageReceiver : BaseLearnGrammarStateReceiver
    {
        public static UserState State => UserState.LearnGrammarMode;

        public override string StateInfo => "*Изучение граматики* 👨‍🎓\n" + GetCommandsDescriptions();

        protected override IEnumerable<StateCommand> InitStateCommands()
        {
            return new[]
            {
               ThemesListCommand,
               ProgressCommand,
               BackToMainCommand
            };
        }

        protected StateCommand ThemesListCommand => new StateCommand
        {
            Key = "/themes",
            Description = "Список тем для изучения",
            Execute = (message, user) => throw new NotImplementedException()
        };

        protected StateCommand ProgressCommand => new StateCommand
        {
            Key = "/progress",
            Description = "Мой прогресс",
            Execute = (message, user) => throw new NotImplementedException()
        };
    }
}
