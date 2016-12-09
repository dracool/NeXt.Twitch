using System;
using System.Threading;
using System.Threading.Tasks;

namespace NeXt.Twitch.Common
{
    /// <summary>
    /// Implements an <see cref="IReconnectStrategy"/> for exponential backoff.
    /// </summary>
    /// <remarks>All Members of this class are Thread-Safe.</remarks>
    /// <seealso cref="IReconnectStrategy"/>
    /// <seealso cref="NoReconnectStrategy"/>
    public sealed class ExponentialBackoffStrategy : IReconnectStrategy
    {
        /// <summary>
        /// Initializes an instance with 10 retries and an intial time of 1 second.
        /// </summary>
        public ExponentialBackoffStrategy()
            :this(10, TimeSpan.FromSeconds(1)) { }

        /// <summary>
        /// Initializes an <see cref="IReconnectStrategy"/> that does exponential backoff.
        /// </summary>
        /// <param name="retries">the amount of retries done</param>
        /// <param name="firstSpan">the initial time span</param>
        public ExponentialBackoffStrategy(int retries, TimeSpan firstSpan)
        {
            rand = new Random();
            Retries = retries;
            FirstSpan = firstSpan;
        }

        private readonly Random rand;

        /// <summary>
        /// The amount of retries done
        /// </summary>
        public int Retries { get; set; }

        /// <summary>
        /// The initial time span used
        /// </summary>
        public TimeSpan FirstSpan { get; set; }

        private void UpdateDuration(ref int duration)
        {
            duration = duration * 2 + rand.Next(-20, +20);
        }

        /// <inheritdoc />
        public bool Execute(Func<bool> action)
        {
            if (action()) return true;

            var current = 0;
            var duration = (int)FirstSpan.TotalMilliseconds;

            while (current < Retries)
            {
                current++;
                Task.Delay(duration).Wait();
                if (action()) return true;
                UpdateDuration(ref duration);
            }
            return false;
        }

        /// <inheritdoc />
        public bool Execute(Func<CancellationToken, bool> action, CancellationToken token)
        {
            if (action(token)) return true;

            var current = 0;
            var duration = (int)FirstSpan.TotalMilliseconds;

            while (current < Retries)
            {
                current++;
                Task.Delay(duration, token).Wait(token);
                if (token.IsCancellationRequested) return false;
                if (action(token)) return true;
                UpdateDuration(ref duration);
            }
            return false;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(Func<Task<bool>> action)
        {
            if (await action()) return true;

            var current = 0;
            var duration = (int)FirstSpan.TotalMilliseconds;

            while (current < Retries)
            {
                current++;
                await Task.Delay(duration);
                if (await action()) return true;
                UpdateDuration(ref duration);
            }
            return false;
        }

        /// <inheritdoc />
        public async Task<bool> ExecuteAsync(Func<CancellationToken, Task<bool>> action, CancellationToken token)
        {
            if (await action(token)) return true;

            var current = 0;
            var duration = (int)FirstSpan.TotalMilliseconds;

            while (current < Retries)
            {
                current++;
                await Task.Delay(duration, token);
                if (token.IsCancellationRequested) return false;
                if (await action(token)) return true;
                UpdateDuration(ref duration);
            }
            return false;
        }
    }
}
