using System;
using System.Threading.Tasks;

namespace NeXt.Twitch.API
{
    public partial class TwitchApi
    {
        /// <summary>
        /// Update the <paramref name="status"/> of a <paramref name="channel"/>.
        /// <para>required scope: <code>channel_editor</code></para>
        /// </summary>
        /// <param name="status">Channel's title.</param>
        /// <param name="channel">The channel to update.</param>
        /// <param name="accessToken">An oauth token with the required scope.</param>
        /// <returns>The response of the request.</returns>
        public async Task<string> UpdateStreamTitle(string status, string channel, string accessToken)
        {
            var data = "{\"channel\":{\"status\":\"" + status + "\"}}";
            return await MakeRestRequest($"https://api.twitch.tv/kraken/channels/{channel}", "PUT", data, accessToken);
        }

        /// <summary>
        /// Update the <paramref name="game"/> the <paramref name="channel"/> is currently playing.
        /// <para>required scope: <code>channel_editor</code></para>
        /// </summary>
        /// <param name="game">Game category to be classified as.</param>
        /// <param name="channel">The channel to update.</param>
        /// <param name="accessToken">An oauth token with the required scope.</param>
        /// <returns>The response of the request.</returns>
        public async Task<string> UpdateStreamGame(string game, string channel, string accessToken)
        {
            var data = "{\"channel\":{\"game\":\"" + game + "\"}}";
            return await MakeRestRequest($"https://api.twitch.tv/kraken/channels/{channel}", "PUT", data, accessToken);
        }

        /// <summary>
        /// Update the <paramref name="status"/> and <paramref name="game"/> of a <paramref name="channel"/>.
        /// <para>required scope: <code>channel_editor</code></para>
        /// </summary>
        /// <param name="status">Channel's title.</param>
        /// <param name="game">Game category to be classified as.</param>
        /// <param name="channel">The channel to update.</param>
        /// <param name="accessToken">An oauth token with the required scope.</param>
        /// <returns>The response of the request.</returns>
        public async Task<string> UpdateStreamTitleAndGame(string status, string game, string channel,
            string accessToken)
        {
            var data = "{\"channel\":{\"status\":\"" + status + "\",\"game\":\"" + game + "\"}}";
            return await MakeRestRequest($"https://api.twitch.tv/kraken/channels/{channel}", "PUT", data, accessToken);
        }

        /// <summary>
        /// Resets the stream key of the <paramref name="channel"/>.
        /// <para>required scope: <code>channel_stream</code></para>
        /// </summary>
        /// <param name="channel">The channel to reset the stream key for.</param>
        /// <param name="accessToken">An oauth token with the required scope.</param>
        /// <returns>The response of the request.</returns>
        public async Task<string> ResetStreamKey(string channel, string accessToken)
        {
            return await
                MakeRestRequest($"https://api.twitch.tv/kraken/channels/{channel}/streamkey", "DELETE", "", accessToken);
        }

        /// <summary>
        /// Updates the <paramref name="delay"/> of a <paramref name="channel"/>.
        /// <para>required scope: <code>channel_editor</code></para>
        /// </summary>
        /// <param name="delay">Channel delay in seconds.</param>
        /// <param name="channel">The channel to update.</param>
        /// <param name="accessToken">The channel owner's access token and the required scope.</param>
        /// <returns>The response of the request.</returns>
        public async Task<string> UpdateStreamDelay(int delay, string channel, string accessToken)
        {
            var data = "{\"channel\":{\"delay\":" + delay + "}}";
            return await MakeRestRequest($"https://api.twitch.tv/kraken/channels/{channel}", "PUT", data, accessToken);
        }

        /// <summary>
        /// Start a commercial on <paramref name="channel"/>.
        /// <para>required scope: <code>channel_commercial</code></para>
        /// </summary>
        /// <param name="length">Length of commercial break in seconds. Default value is 30. You can only trigger a commercial once every 8 minutes.</param>
        /// <param name="channel">The channel to start a commercial on.</param>
        /// <param name="accessToken">An oauth token with the required scope.</param>
        /// <returns>The response of the request.</returns>
        public async Task<string> RunCommercial(CommercialLength length, string channel, string accessToken)
        {
            int seconds;
            switch (length)
            {
                case CommercialLength.Seconds30:
                case CommercialLength.Seconds60:
                case CommercialLength.Seconds90:
                case CommercialLength.Second120:
                case CommercialLength.Seconds150:
                case CommercialLength.Seconds180:
                    seconds = (int)length;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(length));
            }
            return await
                MakeRestRequest($"https://api.twitch.tv/kraken/channels/{channel}/commercial", "POST",
                    $"length={seconds}", accessToken);
        }
    }
}