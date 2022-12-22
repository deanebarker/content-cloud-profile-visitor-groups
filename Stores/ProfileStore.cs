using DeaneBarker.Optimizely.ProfileVisitorGroups;
using JacksonHewitt;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace Alloy.Liquid.Jackson_Hewitt
{
    public class ProfileStore : IProfileStore
    {
        // This is thread-safe; no need to lock
        private static readonly MemoryCache cache = new(new MemoryCacheOptions());

        public Profile Get(string id)
        {
            return cache.Get(id) as Profile;
        }

        public void Put(Profile profile)
        {
            cache.Set(profile.Id, profile);
        }
    }
}
