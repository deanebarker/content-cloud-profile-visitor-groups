namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Stores
{
    public interface IProfileStore
    {
        Profile Get(string id);
        void Put(Profile profile);
    }
}
