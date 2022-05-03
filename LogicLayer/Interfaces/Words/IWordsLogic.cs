﻿using Entities.Common;
using Entities.Navigation;
using Entities.Navigation.InlineMarkupData;
using Telegram.Bot.Types;

namespace LogicLayer.Interfaces
{
    public interface IWordsLogic
    {
        ActionResult LearnWords(UserItem user);
        ActionResult SelectWord(Message message, UserItem user);
        ActionResult ProcessWordResponse(Message message, UserItem user);
        ActionResult StopLearn(UserItem user);
        ActionResult HintWord(UserItem user);
    }
}
