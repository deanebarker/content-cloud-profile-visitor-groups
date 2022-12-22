using DeaneBarker.Optimizely.ProfileVisitorGroups.IdProviders;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.Managers
{
    public interface IProfileManager
    {
        Profile LoadForCurrentUser();
        string GetString(string key);
        int? GetInt(string key);
        DateOnly? GetDate(string key);
        void Save(Profile profile);
        void Update(string id, Dictionary<string, string> data);
    }
}