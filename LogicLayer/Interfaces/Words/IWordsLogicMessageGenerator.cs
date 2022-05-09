using Entities.Common;
using Entities.ConfigSections;
using Entities.Navigation;
using System.Collections.Generic;

namespace LogicLayer.Interfaces.Words
{
    public interface IWordsLogicMessageGenerator
    {
        IWordsConfigSection WordsConfig { get; set; }

        MessageData GetNotEnoughWordsMsg(int notEnoughCount);
        MessageData GetRightAnswerMsg();
        MessageData GetWordLearnedMsg(string word, WordsLearnedCount learnedWords);
        MessageData GetWordRepeatedMsg(string word);
        MessageData GetWordNotFoundMsg();
        MessageData GetWordSuccesfullySelectedMsg(string word);
        MessageData GetRequsetNewWordMsg(IEnumerable<string> notSelectedWords);
        MessageData GetAskWordMsg(WordLearnItem wordForAsking, Language translateFrom, Language translateTo);
        MessageData GetSecondWrongAnswerMsg(WordLearnItem askedWord);
        MessageData GetFirstWrongAnswerMsg();
        MessageData GetAskWordAnswerOptions(string[] answerOptions);
        MessageData GetFirstLevelHint(WordLearnItem askedWord);
        MessageData GetAskWordCallMsg();
        MessageData GetStartLearnMsg();
        MessageData GetStartRepetitionMsg();
        MessageData GetNotRepetitionWordsMsg();
    }
}
