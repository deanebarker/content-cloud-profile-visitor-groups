using DeaneBarker.Optimizely.ProfileVisitorGroups.Managers;
using DeaneBarker.Optimizely.ProfileVisitorGroups.Stores;
using EPiServer.ServiceLocation;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddProfileManager(this IServiceCollection services, Action<ProfileManagerOptions> options)
        {
            services.AddSingleton<IProfileManager, ProfileManager>();
            services.AddSingleton<IProfileStore, ProfileStore>();

            if (options != null)
            {
                services.Configure<ProfileManagerOptions>(options);
            }

            return services;
        }
    }
}
