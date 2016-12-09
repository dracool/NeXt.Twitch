using System;
using System.Collections.Generic;
using System.Linq;
namespace NeXt.Twitch.Chat.Connection
{
    /// <summary>
    /// Use this Attribute to signal the client to pass messages with the specified commands to this method
    /// </summary>
    /// <remarks>
    /// The Commands entered are case sensitive and duplicate commands will cause an exception in the constructor of <see cref="TmiConnection"/>.
    /// the Method signature MUST match <c>void(IrcMessage)</c>. Using any other signature will cause an exception in the constructor of
    /// <see cref="TmiConnection"/>.
    /// </remarks>    
    /// <example>
    /// Create a TmiBasic class that uses the <see cref="IrcCommandAttribute"/> to print
    /// the message text of any <c>"001"</c> irc command to console
    /// <code>
    /// public class TmiBasic : TmiConnection
    /// {
    ///     [IrcCommand("001")]
    ///     private void On001(IrcMessage message)
    ///     {
    ///         Console.WriteLine(message.Raw);
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="TmiConnection"/>
    [AttributeUsage(AttributeTargets.Method)]
    public class IrcCommandAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IrcCommandAttribute"/>
        /// </summary>
        /// <param name="command"></param>
        public IrcCommandAttribute(params string[] command)
        {
            Command = command.ToList().AsReadOnly();
        }

        /// <summary>
        /// The commands this method handles
        /// </summary>
        public IReadOnlyList<string> Command { get; }
    }
}
