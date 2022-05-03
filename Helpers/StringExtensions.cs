﻿using Entities;
using Entities.Common;
using Entities.Navigation;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Helpers
{
    public static class StringExtensions
    {
        public static string Repeat(this string str, int count)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                builder.Append(str);
            }
            return builder.ToString();
        }

        public static MessageData ToMessageData(this string str, IReplyMarkup replyMarkup = null)
        {
            return new MessageData { Text = str, ReplyMarkup = replyMarkup };
        }

        public static ActionResult ToActionResult(this string str, UserState? switchToUserState = null)
        {
            return new ActionResult
            {
                SwitchToUserState = switchToUserState,
                MessagesToSend = new List<MessageData> { new MessageData { Text = str } }
            };
        }
    }
}
