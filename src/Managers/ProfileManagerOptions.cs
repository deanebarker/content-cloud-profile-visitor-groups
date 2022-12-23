using DeaneBarker.Optimizely.ProfileVisitorGroups.Profiles;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups
{
    public class ProfileManagerOptions
    {
        public List<Action<IProfile>> ProfileLoaders { get; set; } = new();
    }
}
