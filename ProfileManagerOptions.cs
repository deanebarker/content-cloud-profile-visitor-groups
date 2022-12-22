﻿namespace DeaneBarker.Optimizely.ProfileVisitorGroups
{
    public class ProfileManagerOptions
    {
        public List<Action<Profile>> ProfileLoaders { get; set; } = new();
        public string CookieName { get; set; }

        public CookieOptions CookieOptions { get; set; } = new();
    }
}
