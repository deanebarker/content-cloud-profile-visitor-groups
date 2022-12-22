using DeaneBarker.Optimizely.ProfileVisitorGroups;
using JacksonHewitt;
using Microsoft.Extensions.Caching.Memory;

namespace Alloy.Liquid.Jackson_Hewitt
{
    public interface IProfileStore
    {
        Profile Get(string id);
        void Put(Profile profile);
    }
}
