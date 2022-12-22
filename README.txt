# Content Cloud Profile Visitor Groups

To enable:

```
services.AddProfileManager();
```

Or:

```
options =>
services.AddProfileManager(options => {
    options.ProfileLoaders.Add(ExternalData.SomeMethodThatPopulatesTheProfile);
    options.CookieName = "theDesiredNameOfTheCookie";
});
```

There are two injected services. They're established in `AddProfileManager`, but can be replace anytime after that:

```
services.AddSingleton<IProfileManager, ProfileManager>();
services.AddSingleton<IProfileStore, ProfileStore>();
```