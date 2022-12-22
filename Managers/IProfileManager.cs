namespace DeaneBarker.Optimizely.ProfileVisitorGroups
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