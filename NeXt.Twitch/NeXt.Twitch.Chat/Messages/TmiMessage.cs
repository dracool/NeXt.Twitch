using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NeXt.Twitch.Chat.Messages
{
    /// <summary>
    /// Represents a parsed tmi message
    /// </summary>
    public class TmiMessage
    {
        /// <summary>
        /// Builder class to build a new <see cref="TmiMessage"/>
        /// </summary>
        public class Builder
        {
            // ReSharper disable once FieldCanBeMadeReadOnly.Local
            private TmiMessage instance;

            /// <summary>
            /// Initializes a new instance of <see cref="Builder"/>
            /// </summary>
            public Builder()
            {
                instance = new TmiMessage();
                Parameters = new List<string>();
            }

            /// <summary>
            /// Builds an <see cref="TmiMessage"/> instance with the information in this builder
            /// </summary>
            /// <returns></returns>
            public TmiMessage Build()
            {
                instance.Parameters = Parameters.AsReadOnly();
                instance.Tags = Tags != null ? new ReadOnlyDictionary<string, string>(Tags) : null;
                return instance;
            }

            /// <summary>
            /// The parameters of the message
            /// </summary>
            public List<string> Parameters { get; }

            /// <summary>
            /// The tags of the message, if any
            /// </summary>
            public IDictionary<string, string> Tags { get; set; }

            /// <summary>
            /// The command of the message
            /// </summary>
            public string Command
            {
                get { return instance.Command; }
                set { instance.Command = value; }
            }

            /// <summary>
            /// The raw message text
            /// </summary>
            public string Raw
            {
                get { return instance.Raw; }
                set { instance.Raw = value; }
            }

            /// <summary>
            /// The raw tags of the message
            /// </summary>
            public string RawTags
            {
                get { return instance.RawTags; }
                set { instance.RawTags = value; }
            }

            /// <summary>
            /// The raw prefix of the message
            /// </summary>
            public string RawPrefix
            {
                get { return instance.RawPrefix; }
                set { instance.RawPrefix = value; }
            }

            /// <summary>
            /// The parsed prefix of the message, if any
            /// </summary>
            public TmiMessagePrefix Prefix
            {
                get { return instance.Prefix; }
                set { instance.Prefix = value; }
            }

        }

        /// <summary>
        /// Creates a new tmi message
        /// </summary>
        private TmiMessage() { }

        /// <summary>
        /// True if the prefix was parsed for this message
        /// </summary>
        public bool PrefixParsed => Prefix != null;

        /// <summary>
        /// True if tags were parsed for this message
        /// </summary>
        public bool TagsParsed => Tags != null;

        /// <summary>
        /// The raw unaltered message passed to the parser
        /// </summary>
        public string Raw { get; private set; }

        /// <summary>
        /// The unparsed tags portion
        /// </summary>
        public string RawTags { get; private set; }

        /// <summary>
        /// The unparsed prefix portion
        /// </summary>
        public string RawPrefix { get; private set; }

        /// <summary>
        /// The parsed prefix if it was parsed, null otherwise
        /// </summary>
        public TmiMessagePrefix Prefix { get; private set; }

        /// <summary>
        /// The irc command
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// The message parameters, inlcuding trailing parameter
        /// </summary>
        public IReadOnlyList<string> Parameters { get; private set; }

        /// <summary>
        /// The parsed tags if they were parsed, null otherwise
        /// </summary>
        public IReadOnlyDictionary<string,string> Tags { get; private set; }
    }
}