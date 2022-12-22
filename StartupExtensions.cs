using Alloy.Liquid.Jackson_Hewitt;
using EPiServer.ServiceLocation;
using JacksonHewitt;
using Microsoft.Extensions.Options;

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
