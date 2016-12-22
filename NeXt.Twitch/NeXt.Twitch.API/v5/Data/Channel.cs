using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeXt.Twitch.API.v5.Data
{
    public class Channel
    {
        [JsonProperty("_id")]
        public int Id { get; private set; }

        [JsonProperty("broadcaster_language")]
        public string BroadcasterLanguage { get; private set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; private set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; private set; }

        [JsonProperty("followers")]
        public int Followers { get; private set; }

        [JsonProperty("game")]
        public string Game { get; private set; }

        [JsonProperty("language")]
        public string Language { get; private set; }

        [JsonProperty("logo")]
        public string Logo { get; private set; }

        [JsonProperty("mature")]
        public bool IsMature { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("partner")]
        public bool IsPartner { get; private set; }

        [JsonProperty("profile_banner")]
        public string ProfileBannerUrl { get; private set; }

        [JsonProperty("profile_banner_background_color")]
        public string ProfileBannerBackgroundColor { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("video_banner")]
        public string VideoBannerUrl { get; private set; }

        [JsonProperty("views")]
        public int Views { get; private set; }
    }
}
