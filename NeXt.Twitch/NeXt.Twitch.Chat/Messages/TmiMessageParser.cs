using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NeXt.Twitch.Chat.Messages
{
    /// <summary>
    /// Class that parses TMI messages into <see cref="TmiMessage"/> instances
    /// </summary>
    public class TmiMessageParser
    {
        /// <summary>
        /// Whether the parser should parse tags into an appropriate Dictionary
        /// </summary>
        public bool ParseTags { get; set; }

        /// <summary>
        /// Whether the prefix should be parsed into a <see cref="TmiMessagePrefix"/> instance
        /// </summary>
        public bool ParsePrefix { get; set; }
        
        /// <summary>
        /// Parses a tmi message string
        /// </summary>
        /// <param name="line">the message to parse</param>
        /// <returns>the parsed message</returns>
        public TmiMessage Parse(string line)
        {
            if (line == null)
                throw new ArgumentNullException(nameof(line));
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentException("Value cannot be empty or whitespace.", nameof(line));

            var message = new TmiMessage.Builder()
            {
                Raw = line,
            };

            int nextspace;
            var position = 0;
            
            if (line[0] == '@')
            {
                nextspace = line.IndexOf(' ');
                if (nextspace == -1)
                {
                    throw new MessageParsingException("Malformed Message: no command after IRCv3 tags");
                }

                message.RawTags = line.Substring(1, nextspace - 1);
                if (ParseTags)
                {
                    message.Tags = DoTagsParsing(message.RawTags);
                }

                position = nextspace + 1;
            }
            else
            {
                message.RawTags = string.Empty;
                if (ParseTags)
                {
                    message.Tags = new Dictionary<string, string>();
                }
            }

            position = SkipSpaces(line, position);
            
            if (line[position] == ':')
            {
                nextspace = line.IndexOf(' ', position);
                if (nextspace == -1)
                {
                    throw new MessageParsingException("Malformed Message: no command after prefix");
                }

                message.RawPrefix = line.Substring(position + 1, (nextspace - position) - 1);

                if (ParsePrefix)
                {
                    message.Prefix = DoPrefixParsing(message.RawPrefix);
                }

                position = nextspace + 1;
                position = SkipSpaces(line, position);
            }
            else
            {
                message.RawPrefix = string.Empty;
                if (ParsePrefix)
                {
                    message.Prefix = new TmiMessagePrefix(string.Empty, string.Empty, string.Empty);
                }
            }
            
            nextspace = line.IndexOf(' ', position);
            if (nextspace == -1)
            {
                if (line.Length > position)
                {
                    message.Command = line.Substring(position);
                }
                return message.Build();
            }
            
            message.Command = line.Substring(position, nextspace - position);
            position = nextspace + 1;
            position = SkipSpaces(line, position);

            while (position < line.Length)
            {
                nextspace = line.IndexOf(' ', position);

                if (line[position] == ':')
                {
                    message.Parameters.Add(line.Substring(position + 1));
                    break;
                }
                
                if (nextspace != -1)
                {
                    message.Parameters.Add(line.Substring(position, nextspace - position));
                    position = nextspace + 1;
                    position = SkipSpaces(line, position);
                    continue;
                }
                
                if (nextspace != -1)
                {
                    continue;
                }

                message.Parameters.Add(line.Substring(position));
                break;
            }

            return message.Build();
        }

        /// <summary>
        /// Parses a tmi message string
        /// </summary>
        /// <param name="line">the message to parse</param>
        /// <param name="message">the message if parsing succeeded, null otherwise</param>
        /// <returns>true if the message successfully parsed, false otherwise</returns>
        public bool TryParse(string line, out TmiMessage message)
        {
            try
            {
                message = Parse(line);
                return true;
            }
            catch(MessageParsingException)
            {
                message = null;
                return false;
            }
        }

        private static IDictionary<string, string> DoTagsParsing(string s)
        {
            return s
                .YieldSplit(';')
                .Select(tag => tag.DivideKeyValue('='))
                .ToDictionary(
                    tuple => tuple.Item1,
                    tuple => tuple.Item2.Length >= 1 ? tuple.Item2 : "true"
                );
        }

        private static TmiMessagePrefix DoPrefixParsing(string s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (string.IsNullOrWhiteSpace(s))
                throw new ArgumentException("prefix cannot be empty or whitespace.", nameof(s));
            if (!s.Contains('@') || !s.Contains('!'))
                throw new MessageParsingException("Malformed Prefix: mask seperators not found");

            var p = s.Split('@', '!');

            if (p.Length < 3)
            {
                return new TmiMessagePrefix(
                    username: string.Empty,
                    nickname: string.Empty,
                    hostname: s
                );
            }

            return new TmiMessagePrefix(
                username: p[0],
                nickname: p[1],
                hostname: p[2]
            );
        }

        private static int SkipSpaces(string text, int position)
        {
            while (position < text.Length && text[position] == ' ')
            {
                position++;
            }

            return position;
        }
    }
}
