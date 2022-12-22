namespace DeaneBarker.Optimizely.ProfileVisitorGroups
{
    public class ProfileManagerOptions
    {
        public List<Action<Profile>> ProfileLoaders { get; set; } = new();
    }
}
