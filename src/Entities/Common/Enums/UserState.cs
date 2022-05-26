namespace Entities
{
    public enum UserState
    {
        WaitingCommand = 0,
        LearnGrammarMode = 1,
        WaitingNewWord = 2,
        WaitingLearnWordResponse = 3,
        LearnWordsMode = 4,
        GrammarTestInProgress = 5,
        WaitingRepetitionWordResponse = 6,
    }
}
