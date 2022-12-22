// Note: this controller is just for the POC, so we can manually set and view profile data
// You won't need this in production

// You can leave it in, and it will just be ignored for Release builds

#if DEBUG

using DeaneBarker.Optimizely.ProfileVisitorGroups.Managers;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using System.Text;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.TestingCode
{
    [Route("profile")]
    public class ProfileController : Controller
    {
        private IProfileManager _profileManager;

        public ProfileController(IProfileManager profileManager)
        {
            _profileManager = profileManager;
        }

        [Route("set")]
        public string Set(string key, string value)
        {
            var profile = _profileManager.LoadForCurrentUser();

            if (value == null)
            {
                profile.Remove(key, out string _);
            }
            else
            {
                profile[key] = value;
            }
            _profileManager.Save(profile);

            Response.ContentType = "text/plain";
            return profile.ToString();
        }


        [Route("show")]
        public string Show()
        {
            var profile = _profileManager.LoadForCurrentUser();
            Response.ContentType = "text/plain";

            var sb = new StringBuilder();
            sb.Append(profile.ToString());
            sb.AppendLine();

            var vgs = ServiceLocator.Current.GetInstance<IVisitorGroupRepository>();
            foreach(var vg in vgs.List())
            {
                sb.AppendLine($"\"{vg.Name}\"");
                sb.AppendLine(new string('-', vg.Name.Length + 2));
                foreach(var criterion in vg.Criteria)
                {
                    // This is awful code.
                    // Get vaccinated just because you looked at it.
                    // (There has to be a better way of doing this...)
                    var executable = Activator.CreateInstance(Type.GetType(criterion.TypeName));
                    executable.GetType().GetMethod("Initialize").Invoke(executable, new object[] { criterion });
                    var result = executable.GetType().GetMethod("IsMatch").Invoke(executable, new object[] { (IPrincipal)null, (HttpContext)null });

                    sb.AppendLine($"{result.ToString().ToUpper()}: {executable.GetType().Name}, {getCriterionAsString(criterion.Model)}");
                }
                sb.AppendLine();
            }

            return sb.ToString();

            string getCriterionAsString(ICriterionModel model)
            {
                return string.Join(", ", model.GetType().GetProperties().Where(p => p.Name != "Id").Select(p => $"{p.Name}:\"{p.GetValue(model)}\""));
            }
        }


        // Shows all the profiles currently stored
        [Route("all")]
        public string All()
        {
            var sb = new StringBuilder();
            foreach (var profile in ((ProfileManager)_profileManager).GetAll())
            {
                sb.AppendLine(profile.Id);
                sb.AppendLine(new string('-', profile.Id.Length));
                sb.Append(profile);
                sb.AppendLine();
            }
            Response.ContentType = "text/plain";
            return sb.ToString();
        }
    }
}

#endif