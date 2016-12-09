using System;
using System.Threading;
using System.Threading.Tasks;

namespace NeXt.Twitch.Common
{
    /// <summary>
    /// A <see cref="IReconnectStrategy"/> implementation that immediately fails to reconnect
    /// </summary>
    /// <remarks>This Type is Thread Safe</remarks>
    /// <seealso cref="IReconnectStrategy"/>
    /// <seealso cref="ExponentialBackoffStrategy"/>
    public sealed class NoReconnectStrategy : IReconnectStrategy
    {
        private NoReconnectStrategy() { }

        private static readonly Lazy<NoReconnectStrategy> LazyInstance = new Lazy<NoReconnectStrategy>(
            () => new NoReconnectStrategy(),
            LazyThreadSafetyMode.PublicationOnly
        );

        /// <summary>
        /// An instance of <see cref="IReconnectStrategy"/> that always fails when reconnecting
        /// </summary>
        public static IReconnectStrategy Instance => LazyInstance.Value;

        /// <inheritdoc />
        public bool Execute(Func<bool> action)
        {
            return false;
        }

        /// <inheritdoc />
        public bool Execute(Func<CancellationToken, bool> action, CancellationToken token)
        {
            return false;
        }

        /// <inheritdoc />
        public Task<bool> ExecuteAsync(Func<Task<bool>> action)
        {
            return Task.FromResult(false);
        }

        /// <inheritdoc />
        public Task<bool> ExecuteAsync(Func<CancellationToken, Task<bool>> action, CancellationToken token)
        {
            return Task.FromResult(false);
        }
    }
}