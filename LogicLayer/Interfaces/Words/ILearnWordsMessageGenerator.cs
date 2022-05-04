using Entities.Common;
using System.Collections.Generic;

namespace LogicLayer.Interfaces.Words
{
    public interface ILearnWordsMessageGenerator
    {
        MessageData GetNotEnoughWordsMsg(int notEnoughCount);
        MessageData GetRightAnswerMsg();
        MessageData GetWordLearnedMsg(string word, WordsLearned learnedWords);
        MessageData GetWordNotFoundMsg();
        MessageData GetWordSuccesfullySelectedMsg(string word);
        MessageData GetRequsetNewWordMsg(IEnumerable<string> notSelectedWords);
        MessageData GetAskWordMsg(WordLearnItem wordForAsking, Language translateFrom, Language translateTo);
        MessageData GetSecondWrongAnswerMsg(WordLearnItem askedWord);
        MessageData GetFirstWrongAnswerMsg();
        MessageData GetAskWordAnswerOptions(string[] answerOptions);
        MessageData GetFirstLevelHint(WordLearnItem askedWord);
        MessageData GetAskWordCallMsg();
    }
}
