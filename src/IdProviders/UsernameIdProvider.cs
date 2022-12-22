using DeaneBarker.Optimizely.ProfileVisitorGroups.IdProviders;

public class UsernameIdProvider : IIdProvider
{
    private IHttpContextAccessor _httpContextAccessor;

    public UsernameIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetId()
    {
        return _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}

