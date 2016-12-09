using System;
using System.Threading;
using System.Threading.Tasks;

namespace NeXt.Twitch.Common
{
    /// <summary>
    /// Describes a strategy for reconnecting to a service
    /// </summary>
    public interface IReconnectStrategy
    {
        /// <summary>
        /// Executes the reconnect strategy, the reconnect is successful if <paramref name="action"/> returns true
        /// <para>WARNING: this will block until the reconnected succeeded or permanently failed</para>
        /// </summary>
        /// <param name="action">the function to execute on each try</param>
        /// <returns>true if the reconnect was sucessful, false otherwise</returns>
        bool Execute(Func<bool> action);

        /// <summary>
        /// Executes the reconnect strategy, the reconnect is successful if <paramref name="action"/> returns true
        /// <para>WARNING: this will block until the reconnect ends or the cancellation token is set</para>
        /// <para>NOTE: The CancellationToken may be used to stop between delegate calls, do not rely on it's state in the call</para>
        /// </summary>
        /// <param name="action">the function to execute on each try</param>
        /// <param name="token">the cancellation token passed to the function</param>
        /// <returns>true if the reconnect was sucessful, false otherwise</returns>
        bool Execute(Func<CancellationToken, bool> action, CancellationToken token);

        /// <summary>
        /// Asynchronously executes the reconnect strategy, the reconnect is successful if <paramref name="action"/> returns true
        /// </summary>
        /// <param name="action">the function to execute on each try</param>
        /// <returns>true if the reconnect was sucessful, false otherwise</returns>
        Task<bool> ExecuteAsync(Func<Task<bool>> action);
        
        /// <summary>
        /// Asynchronously executes the reconnect strategy, the reconnect is successful if <paramref name="action"/> returns true
        /// <para>NOTE: The CancellationToken may be used to stop between delegate calls, do not rely on it's state in the call</para>
        /// </summary>
        /// <param name="action">the delegate to execute on each try</param>
        /// <param name="token">the cancellation token passed to the function</param>
        /// <returns>true if the reconnect was succesful, false otherwise</returns>
        Task<bool> ExecuteAsync(Func<CancellationToken, Task<bool>> action, CancellationToken token);
    }
}