using Entities.Common;
using Entities.Common.Grammar;
using System.Collections.Generic;

namespace LogicLayer.Interfaces.Grammar
{
    public interface ITestAccessorMessageGenerator
    {
        MessageData GetThemesListMsg(List<UserThemeItem> userThemes);
        MessageData GetShowThemeMsg(UserThemeExtendedItem userThemeItem);
    }
}
