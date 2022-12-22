using DeaneBarker.Optimizely.ProfileVisitorGroups.Managers;
using EPiServer.ServiceLocation;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.TestingCode
{
    public static class ExternalData
    {
        // Some external system
        public static void PopulateProfileFromExternalSource_One(this Profile profile)
        {
            profile["first_name"] = "Deane";
            profile["last_name"] = "Barker";
            profile["dob"] = "1971-09-03";
            profile["annual_salary"] = "10000";
            profile["state"] = "SD";
            profile["country"] = "USA";
            profile["last_visited"] = "2022-12-01";
        }

        // Some other external system
        public static void PopulateProfileFromExternalSource_Two(this Profile profile)
        {
            profile["num_of_children"] = "3";
            profile["favorite_singer"] = "Taylor Swift";
        }

        // Some really slow external system
        public static void PopulateProfileFromExternalSource_Three(this Profile profile)
        {
            // Simulates a long-running operation
            // This will come back and update the profile later
            // Visitor Group assignment will adjust in real-time
            Task.Run(() =>
            {
                Thread.Sleep(10000);

                var data = new Dictionary<string, string>
                {
                    ["dogs_name"] = "Lavallette"
                };

                var profileManager = ServiceLocator.Current.GetInstance<IProfileManager>();
                profileManager.Update(profile.Id, data);                
            });
        }
    }
}
