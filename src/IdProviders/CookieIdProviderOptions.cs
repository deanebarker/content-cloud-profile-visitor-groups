namespace DeaneBarker.Optimizely.ProfileVisitorGroups.IdProviders
{
    public class CookieIdProviderOptions
    {
        public string CookieName { get; set; } = "contentcloudprofileid";
        public CookieOptions CookieOptions { get; set; } = new CookieOptions();
    }
}
