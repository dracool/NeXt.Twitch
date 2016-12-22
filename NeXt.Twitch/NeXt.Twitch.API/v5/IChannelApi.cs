using System.Collections.Generic;
using System.Threading.Tasks;
using NeXt.Twitch.API.v5.Data;

namespace NeXt.Twitch.API.v5
{
    public interface IChannelApi
    {
        Task<AuthenticatedChannel> Get();
        Task<Channel> Get(long id);
        Task<IReadOnlyList<User>> GetEditors(long id);
        Task<Channel> Update(long id, ChannelUpdate arguments);
    }
}