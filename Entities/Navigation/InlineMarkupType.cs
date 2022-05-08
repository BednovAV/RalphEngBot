using System.ComponentModel;

namespace Entities.Navigation
{
    public enum InlineMarkupType
    {
        [Description("Выйти")]
        ExitFromWordsLearning,
        [Description("Подсказка")]
        WordHint,
        SwitchShowUserWordPage,
        [Description("Вернуться")]
        BackToLearnGrammarMode,
        GoToTheme,
        [Description("Вернуться к списку тем")]
        GoToThemeList,
        [Description("Пройти тест")]
        StartTest,
        [Description("Завершить тест")]
        CompleteTest,
        GiveAnswer,
        [Description("Выйти")]
        ExitFromTest,
    }
}
