using Entities.Common;
using System.Collections.Generic;

namespace LogicLayer.Interfaces.Words
{
    public interface IWordsMessageGenerator
    {
        MessageData GetNotEnoughWordsMsg(int notEnoughCount);
        MessageData GetRightAnswerMsg();
        MessageData GetWordLearnedMsg(string word);
        MessageData GetWordNotFoundMsg();
        MessageData GetWordSuccesfullySelectedMsg(string word);
        MessageData GetRequsetNewWordMsg(IEnumerable<string> notSelectedWords);
        MessageData GetAskWordMsg(WordLearnItem wordForAsking, Language translateFrom, Language translateTo);
        MessageData GetSecondWrongAnswerMsg(WordLearnItem askedWord);
        MessageData GetFirstWrongAnswerMsg();
        MessageData GetAskWordAnswerOptions(string[] answerOptions);
    }
}
