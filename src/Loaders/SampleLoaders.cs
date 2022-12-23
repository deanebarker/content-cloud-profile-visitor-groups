using DeaneBarker.Optimizely.ProfileVisitorGroups.Managers;
using DeaneBarker.Optimizely.ProfileVisitorGroups.Profiles;
using EPiServer.ServiceLocation;
using System.Text.Json;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.TestingCode
{
    public static class SampleLoaders
    {
        // Some external system
        public static void PopulateProfileFromExternalSource_One(IProfile profile)
        {
            profile["first_name"] = "Deane";
            profile["last_name"] = "Barker";
            profile["dob"] = "1971-09-03";
            profile["annual_salary"] = "10000";
            profile["state"] = "SD";
            profile["country"] = "USA";
            profile["last_visited"] = "2022-12-01";
        }

        // Some really slow external system
        public static void PopulateProfileFromExternalSource_Two(IProfile profile)
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

        public static void PopulateProfileFromFakeWebServiceCall(IProfile profile)
        {
            // Pretend this is a web service call to some external system
            // It would pass the value of Profile.Id as a parameter
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"App_Data/Profiles/{profile.Id}.json");
            var json = File.ReadAllText(path);

            ((JsonProfile)profile).LoadJson(json);
        }
    }
}
