using DeaneBarker.Optimizely.ProfileVisitorGroups.IdProviders;
using DeaneBarker.Optimizely.ProfileVisitorGroups.Profiles;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Managers
{
    public interface IProfileManager
    {
        IProfile LoadForCurrentUser();
        string GetString(string key);
        int? GetInt(string key);
        DateOnly? GetDate(string key);
        void Save(IProfile profile);
        void Update(string id, Dictionary<string, string> data);
    }
}