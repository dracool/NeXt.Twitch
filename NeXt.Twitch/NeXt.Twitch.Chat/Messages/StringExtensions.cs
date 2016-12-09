using System;
using System.Collections.Generic;

namespace NeXt.Twitch.Chat.Messages
{
    internal static class StringExtensions
    {
        private static string IndexSubstring(string s, int start, int end)
        {
            return s.Substring(start, end - start);
        }

        public static IEnumerable<string> YieldSplit(this string s, char delimiter)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException("Value cannot be empty or whitespace.", nameof(s));

            var cIndex = 0;
            int nIndex;
            do
            {
                nIndex = s.IndexOf(delimiter, cIndex);
                //omit empty
                if (nIndex > cIndex + 1)
                {
                    yield return IndexSubstring(s, cIndex, nIndex);
                }
                cIndex = nIndex + 1;
            } while (nIndex >= 0);
        }

        public static Tuple<string, string> DivideKeyValue(this string s, char delimiter)
        {
            var i = s.IndexOf(delimiter);
            var left = s.Substring(0, i);
            var right = s.Substring(i + 1);

            return new Tuple<string,string>(left, right);
        }
    }
}
