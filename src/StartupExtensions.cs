using DeaneBarker.Optimizely.ProfileVisitorGroups.IdProviders;
using DeaneBarker.Optimizely.ProfileVisitorGroups.Managers;
using DeaneBarker.Optimizely.ProfileVisitorGroups.Profiles;
using DeaneBarker.Optimizely.ProfileVisitorGroups.Stores;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.DependencyInjection;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddProfileVisitorGroups(this IServiceCollection services, Action<ProfileManagerOptions> options)
        {
            services.AddSingleton<IProfileManager, ProfileManager>();
            services.AddSingleton<IProfileStore, ProfileStore>();
            services.AddTransient<IProfile, DictionaryProfile>();

            if (options != null)
            {
                services.Configure<ProfileManagerOptions>(options);
            }

            services.AddCookieIdProvider();

            return services;

        }

        public static IServiceCollection AddCookieIdProvider(this IServiceCollection services, Action<CookieIdProviderOptions> options = null)
        {
            services.AddSingleton<IIdProvider, CookieIdProvider>();

            if (options != null)
            {
                services.Configure<CookieIdProviderOptions>(options);
            }

            return services;

        }

        public static IServiceCollection AddUsernameIdProvider(this IServiceCollection services)
        {
            services.AddSingleton<IIdProvider, UsernameIdProvider>();
            return services;
        }

        public static IServiceCollection AddJsonProfile(this IServiceCollection services)
        {
            services.AddTransient<IProfile, JsonProfile>();
            return services;
        }
    }
}
