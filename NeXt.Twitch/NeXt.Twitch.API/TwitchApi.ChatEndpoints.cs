using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace NeXt.Twitch.API
{
    public partial class TwitchApi
    {
        private static List<DnsEndPoint> EndpointsResponseToList(string response)
        {
            return JObject.Parse(response)
                .SelectToken("chat_servers")
                .Select(t => t?.ToString())
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => s.Split(':'))
                .Select(sa => new DnsEndPoint(sa[0], int.Parse(sa[1])))
                .ToList();
        }

        /// <summary>
        /// Gets available chat servers for the specified channel
        /// </summary>
        /// <param name="channel">the channel to get chat servers for</param>
        /// <returns>list of ip endpoints</returns>
        public async Task<List<DnsEndPoint>> GetChatServers(string channel)
        {
            return EndpointsResponseToList(await MakeGetRequest($"https://api.twitch.tv/api/channels/{channel}/chat_properties"));
            
        }

        /// <summary>
        /// Gets available chat servers for whispering
        /// </summary>
        /// <returns>list of ip endpoints</returns>
        public async Task<List<DnsEndPoint>>  GetWhisperServers()
        {
            return EndpointsResponseToList(await MakeGetRequest("http://tmi.twitch.tv/servers?cluster=group"));
        }
    }
}
