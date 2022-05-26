using Entities;
using Entities.Navigation;
using Helpers;
using LogicLayer.Interfaces.Grammar;
using System;
using System.Collections.Generic;

namespace Communication
{
    public class LearnGrammarMessageReceiver : BaseLearnGrammarStateReceiver
    {
        private readonly IGrammarTestAccessor _grammarTestAccessor;

        public LearnGrammarMessageReceiver(IGrammarTestAccessor grammarTestAccessor)
        {
            _grammarTestAccessor = grammarTestAccessor;
        }

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
            Execute = (message, user) => _grammarTestAccessor.ShowThemes(user).ToActionResult()
        };

        protected StateCommand ProgressCommand => new StateCommand
        {
            Key = "/progress",
            Description = "Мой прогресс",
            Execute = (message, user) => throw new NotImplementedException()
        };
    }
}
