using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using NeXt.Twitch.Chat.Messages;

namespace NeXt.Twitch.Chat.Connection
{
    public partial class TmiConnection
    {
        /// <summary>
        /// The message parser used to parse raw messages in <see cref="OnRawReceived"/>.
        /// </summary>
        /// <remarks>
        /// This is only marked as protected to allow configuration of the parser. Invoking
        /// the parser directly should not be necessary, see example on <see cref="TmiConnection"/>.
        /// </remarks>
        // ReSharper disable once UnassignedReadonlyField (weird error in ReSharper, guess it has to do with partial class)
        protected readonly TmiMessageParser Parser;

        /// <summary>
        /// The commands registered for this instance
        /// </summary>
        private IReadOnlyDictionary<string, Action<TmiMessage>> commandRegistry;

        private void RegisterCommands()
        {
            var data = new Dictionary<string, Action<TmiMessage>>();

            var t = GetType();

            foreach (var minfo in t.GetMethods(
                      BindingFlags.InvokeMethod 
                    | BindingFlags.Instance 
                    | BindingFlags.NonPublic 
                    | BindingFlags.Public)
                .Where(m => m.IsDefined(typeof(IrcCommandAttribute))))
            {
                var attrib = minfo.GetCustomAttribute<IrcCommandAttribute>();
                if (attrib.Command.Count <= 0) continue;
                var para = minfo.GetParameters();

                if (minfo.ReturnType != typeof(void))
                    throw new InvalidOperationException($"Command handler handler does not have signature void(IrcMessage): {minfo.Name}");
                if (para.Length != 1)
                    throw new InvalidOperationException($"Command handler handler does not have signature void(IrcMessage): {minfo.Name}");
                if (para[0].ParameterType != typeof(TmiMessage))
                    throw new InvalidOperationException($"Command handler does not have signature void(IrcMessage): {minfo.Name}");
                
                var deleg = (Action<TmiMessage>)minfo.CreateDelegate(typeof(Action<TmiMessage>), this);

                foreach (var cmd in attrib.Command)
                {
                    if (data.ContainsKey(cmd)) throw new InvalidOperationException("duplicate command handler for " + cmd);
                    data.Add(cmd, deleg);
                }
            }

            commandRegistry = new ReadOnlyDictionary<string, Action<TmiMessage>>(data);
        }

        /// <summary>
        /// Called when a raw text was received. Only override if you need control over the message parsing
        /// and forwarding process. This should normally not be necessary.
        /// </summary>
        /// <param name="raw">the raw text</param>
        protected virtual void OnRawReceived(string raw)
        {
            RawReceived?.Invoke(this, raw);

            TmiMessage msg = null;
            var success = false;
            try
            {
                msg = Parser.Parse(raw);
                success = true;
            }
            catch (Exception e)
            {
                if (Invalid == null) throw;
                OnInvalid(new InvalidMessageEventArgs(raw, e));
            }
            if (!success) return;

            Action<TmiMessage> handler;
            if (commandRegistry.TryGetValue(msg.Command, out handler))
            {
                handler(msg);
            }
            else
            {
                OnUnknown(msg);
            }
        }

        /// <summary>
        /// Called when a message was received that failed to parse.
        /// </summary>
        /// <param name="e">the event args.</param>
        protected virtual void OnInvalid(InvalidMessageEventArgs e)
        {
            Invalid?.Invoke(this, e);
        }

        /// <summary>
        /// Called when a message was received that did not have a handler 
        /// associated with it's command.
        /// </summary>
        /// <param name="msg">the message received.</param>
        protected virtual void OnUnknown(TmiMessage msg)
        {
            Unknown?.Invoke(this, new UnknownMessageEventArgs(msg));
        }

    }
}