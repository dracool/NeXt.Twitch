using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeXt.Twitch.API.v5
{
    internal abstract class ApiCategory
    {
        protected ApiCategory(TwitchApi api)
        {
            API = api;
        }

        protected TwitchApi API { get; private set; }
    }
}
