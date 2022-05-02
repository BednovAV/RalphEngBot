using Entities.Common;
using System.Text;

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

        public static MessageData ToMessageData(this string str, bool removeKeyboard = true)
        {
            return new MessageData { Text = str, RemoveKeyboard = removeKeyboard };
        }
    }
}
