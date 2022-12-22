using DeaneBarker.Optimizely.ProfileVisitorGroups.IdProviders;
using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DeaneBarker.Optimizely.ProfileVisitorGroups.IdProviders
{
    public class CookieIdProvider : IIdProvider
    {
        private IHttpContextAccessor _httpContextAccessor;
        private CookieIdProviderOptions _options;
        private string httpContextKey = "cookieProfileBinderContextKey";

        public CookieIdProvider(IHttpContextAccessor httpContextAccessor, IOptions<CookieIdProviderOptions> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _options = options.Value;
        }

        public string GetId()
        {
            var httpContextAccessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();

            string id;

            var cookie = httpContextAccessor.HttpContext.Request.Cookies[_options.CookieName];
            if (cookie != null)
            {
                // Get the ID that was passed IN
                id = cookie.ToString();
            }
            else
            {
                // There's no cookie, so...

                // Check to see if it's stored as an HttpContext item
                // The problem is that during the first request, this id is not in a cookie, so it's not globally available
                // So for the first request, we have to force it to be globally available. After this, we send it back as a cookie, and we're good
                if (httpContextAccessor.HttpContext.Items[httpContextKey] != null)
                {
                    id = httpContextAccessor.HttpContext.Items[httpContextKey].ToString();
                }
                else
                {
                    // Create a new ID and passed it BACK
                    id = Guid.NewGuid().ToString();
                    httpContextAccessor.HttpContext.Response.Cookies.Append(_options.CookieName, id, _options.CookieOptions);
                    // Note: I haven't manually done anything with cookies in YEARS
                    // Is this persistent? I think so? If not, you'll need to add some CookieOptions settings to make it persistent

                    // Put it in the context so it's globally available for the entirety of the request
                    httpContextAccessor.HttpContext.Items[httpContextKey] = id;
                }

            }

            return id;
        }
    }
}
