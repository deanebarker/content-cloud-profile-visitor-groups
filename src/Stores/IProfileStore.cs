using DeaneBarker.Optimizely.ProfileVisitorGroups.Profiles;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Stores
{
    public interface IProfileStore
    {
        IProfile Get(string id);
        void Put(IProfile profile);
    }
}
