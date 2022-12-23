namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Profiles
{
    public interface IProfile
    {
        string Id { get; set; }

        string Get(string keys);

        void Remove(string key);

        string this[string key]
        {
            get;
            set;
        }
    }
}