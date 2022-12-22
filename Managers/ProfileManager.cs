using EPiServer.ServiceLocation;
using Microsoft.Extensions.Caching.Memory;
using Alloy.Liquid.Jackson_Hewitt;
using Microsoft.Extensions.Options;
using DeaneBarker.Optimizely.ProfileVisitorGroups;

namespace JacksonHewitt
{
    public class ProfileManager : IProfileManager
    {
        private string cookieName;

        public static List<Action<Profile>> ProfileLoaders { get; set; } = new();
        private readonly IProfileStore _store;

        public ProfileManager(IProfileStore store, IOptions<ProfileManagerOptions> options)
        {
            _store = store;
            ProfileLoaders.AddRange(options.Value.ProfileLoaders);
            cookieName = options.Value.CookieName ?? "_profileManagerCookie";
        }

        public Profile Load(string id)
        {
            return _store.Get(id);
        }

        public virtual Profile LoadForCurrentUser()
        {
            var id = GetIdFromCookie();

            var profile = Load(id);
            if (profile == null)
            {
                // Create a new profile
                // This constructor will populate from the CDP
                profile = new Profile()
                {
                    Id = id
                };

                foreach (var loader in ProfileLoaders)
                {
                    loader.Invoke(profile);
                }

                Save(profile);

                #if DEBUG
                Manifest.Add(id);
                #endif
            }

            return profile;
        }

        public void Update(string id, Dictionary<string, string> data)
        {
            var profile = Load(id);
            if (profile == null) return;

            foreach (var datum in data)
            {
                profile[datum.Key] = datum.Value;
            }

            Save(profile);
        }


        public string GetString(string key)
        {
            var profile = LoadForCurrentUser();
            return profile.Get(key);
        }


        public int? GetInt(string key)
        {
            var value = GetString(key);
            if (value == null)
            {
                return null;
            }

            if (!int.TryParse(value, out int typedValue))
            {
                return null;
            }

            return typedValue;
        }

        public DateOnly? GetDate(string key)
        {
            var value = GetString(key);
            if (value == null)
            {
                return null;
            }

            if (!DateOnly.TryParse(value, out DateOnly typedValue))
            {
                return null;
            }

            return typedValue;
        }


        public void Save(Profile profile)
        {
            _store.Put(profile);
        }


        // Returns one of...
        // 1. The value of the identifier cookie that was passed IN
        // 2. The value of a new identifier cookie that will be passed BACK
        private string GetIdFromCookie()
        {
            var httpContext = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();

            string id;

            var cookie = httpContext.HttpContext.Request.Cookies[cookieName];
            if (cookie != null)
            {
                // Get the ID that was passed IN
                id = cookie.ToString();
            }
            else
            {
                // Create a new ID and passed it BACK
                id = Guid.NewGuid().ToString();
                httpContext.HttpContext.Response.Cookies.Append(cookieName, id);

            }

            return id;
        }

#if DEBUG
                
        // Won't need for prodction
        // This is just so we can list the profiles for /profile/all
        private List<string> Manifest = new(); // This exists just to keep track of the keys in the cache (we can't iterate cache keys)
        public List<Profile> GetAll()
        {
            return Manifest.Select(k => _store.Get(k) as Profile).ToList();
        }

#endif
    }
}
