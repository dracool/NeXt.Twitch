using NeXt.Twitch.API.v5.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeXt.Twitch.API.v5
{
    internal class ChannelApi : ApiCategory, IChannelApi
    {
        public ChannelApi(TwitchApi api) : base(api) { }

        //GET 
        /// <summary>
        /// 
        ///  <para>Required scope: channel_read</para>
        /// </summary>
        /// <returns></returns>
        public async Task<AuthenticatedChannel> Get()
        {
            return API.Deserialize<AuthenticatedChannel>(await API.MakeGetRequest(@"https://api.twitch.tv/kraken/channels"));
        }

        //GET https://api.twitch.tv/kraken/channels/<channel_ID>
        /// <summary>
        /// 
        /// <para>Required scope: none</para>
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<Channel> Get(long id)
        {
            return API.Deserialize<Channel>(await API.MakeGetRequest($"https://api.twitch.tv/kraken/channels/{id}"));
        }

        //PUT https://api.twitch.tv/kraken/channels/<channel_ID>
        /// <summary>
        /// 
        ///  <para>Required scope: channel_read</para>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public async Task<Channel> Update(long id, ChannelUpdate arguments)
        {
            throw new NotImplementedException();
        }

        //GET https://api.twitch.tv/kraken/channels/<channel ID>/editors
        /// <summary>
        /// 
        ///  <para>Required scope: channel_read</para>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<User>> GetEditors(long id)
        { 
            return API.Deserialize<IReadOnlyList<User>>(await API.MakeGetRequest($"https://api.twitch.tv/kraken/channels/{id}/editors"));
        }
    }

    public sealed class ChannelUpdate
    {
        private ChannelUpdate() { }

        public string Status { get; private set; }
        public string Game { get; private set; }
        public string Delay { get; private set; }
        public bool? IsChannelFeedEnabled { get; private set; }

        public ChannelUpdate WithStatus(string status)
        {
            Status = status;
            return this;
        }

        public ChannelUpdate WithGame(string game)
        {
            Game = game;
            return this;
        }

        public ChannelUpdate WithDelay(TimeSpan delay)
        {
            Delay = ((int)delay.TotalSeconds).ToString();
            return this;
        }

        public ChannelUpdate WithDelay(int seconds)
        {
            Delay = seconds.ToString();
            return this;
        }

        public ChannelUpdate WithChannelFeed(bool enable)
        {
            IsChannelFeedEnabled = enable;
            return this;
        }

        public ChannelUpdate WithChannelFeedEnabled()
        {
            IsChannelFeedEnabled = true;
            return this;
        }

        public ChannelUpdate WithChannelFeedDisabled()
        {
            IsChannelFeedEnabled = true;
            return this;
        }

        public static ChannelUpdate CreateWithStatus(string status)
        {
            return new ChannelUpdate().WithStatus(status);
        }

        public static ChannelUpdate CreateWithGame(string game)
        {
            return new ChannelUpdate().WithGame(game);
        }

        public static ChannelUpdate CreateWithDelay(TimeSpan delay)
        {
            return new ChannelUpdate().WithDelay(delay);
        }

        public static ChannelUpdate CreateWithDelay(int seconds)
        {
            return new ChannelUpdate().WithDelay(seconds);
        }

        public static ChannelUpdate CreateWithChannelFeed(bool enable)
        {
            return new ChannelUpdate().WithChannelFeed(enable);
        }

        public static ChannelUpdate CreateWithChannelFeedEnabled()
        {
            return new ChannelUpdate().WithChannelFeedEnabled();
        }

        public static ChannelUpdate CreateWithChannelFeedDisabled()
        {
            return new ChannelUpdate().WithChannelFeedDisabled();
        }
    }
}
