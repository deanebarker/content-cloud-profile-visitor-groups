# Content Cloud Profile Visitor Groups

This library allows a key/value store to be instantiated and bound to a cookie send with the user's request. This key-value store can be injected with data representing a user's demographic/profile information.

This library provides five different Visitor Group criteria to query information in this profile. For each, a key can be specified, and the value for this key will be retrieved from the profile to provide the basis for comparison.

* **Text:** compare if a text profile value equals, starts with, ends with, contains, (etc.) a provided number
  >Determine if the value for "email_address" ends with "@potential-customer.com"
* **Number:** compare if a numeric profile value equals, is greater than, is less than, (etc.) a provided number
  >Determine if the value for "number_of_children" is greater than 0.
* **Date:** compare if a dated numeric profile value equals, is greater than, is less than, (etc.) a provider date 
  >Determine if the value for "date_accounted_created" is prior to January 1, 2022.
* **Relative Date:** compare if a specified part of the timespan between a dated profile value and now equals, is greater than, is less than, (etc.) a provided number
  >Determine if the number of years between now and the value for "date_of_birth" is greater than 18.
* **Exists:** determine if a key does or does not exist (regardless of value)
  >Determine if a key for "account_suspended_date" exists.

These criteria can be combined to define granular Visitor Groups based on profile information.

## Adding Data to a Profile

The profile can be access via `ProfileManager`, which can be used to inject data. The `Profile` object is simply a `ConcurrentDictionary`.

```
var profileManager = ServiceLocation.Current.GetInstance<IProfileManager>();
var profile = profileManager.LoadForCurrentUser();
profile["some_key"] = "some value";
profilerManager.Save(profile)
```

If a profile doesn't exist for this visitor, it will be automatically created and bound to their request with a persistent cookie.

Alternately, `ProfileLoaders` can be specified which will automatically load profiles with data upon creation.

```
ProfileManager.ProfileLoader(GetExternalData);

public static void GetExternalData(Profile profile)
{
  profile["some_key"] = "some value";
}

```

## Installing

To enable:

```
services.AddProfileManager();
```

Or:

```
services.AddProfileManager(options => {
    options.ProfileLoaders.Add(ExternalData.SomeMethodThatPopulatesTheProfile);
    options.CookieName = "theDesiredNameOfTheCookie";
});
```

There are two injected services. They're established in `AddProfileManager`, but can be replaced anytime after that:

```
services.AddSingleton<IProfileManager, ProfileManager>();
services.AddSingleton<IProfileStore, ProfileStore>();
```

You'll also need to register `IHttpContextAccessor`

```
services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
```


## Testing Controller

`ProfileController` can be used to test the profiles.

* **/profile/show** will show the profile for the current user and how that profile is performing against all profile criteria in all visitor groups.
* **/profile/set** will allow manual setting of profile data via querystring: `/profile/set?key=first_name&value=deane`. Not supplying a value will cause that key to be deleted.
* **/profile/all** will show all profiles current in the system