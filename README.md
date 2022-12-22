# Content Cloud Profile Visitor Groups

## Core Use Case

In general terms:

>You have some external store of profile information, be it an actual CDP or something else. This is fairly static, non-volatile, demographic information that doesn't change based on immediate visitor behavior (examples: first name, email address, date-of-birth, etc.). You want a way to bind this data to the user's session, and use it to personalize content.

In Optimizely Content Cloud terms:

>You want a set of Visitor Group Criteria by which you can query a store of information about _this specific user_ and show or hide content based on the results. This store of information can be anything, so you need the criteria to be generic and universally applicable. And you don't want the overhead of querying the external data source every time, because the data doesn't change that often, so you just need it cached and available.

(Note: if you're using the Optimizely Data Platform, you don't need this. Our built-in integration handles all this already.)

## Details

This library allows a key/value store to be instantiated and bound to a cookie send with the user's request. This key-value store can be injected with data representing a user's demographic/profile information.

This library provides five different Visitor Group criteria to query information in this profile. For each, a key can be specified, and the value for this key will be retrieved from the profile to provide the basis for comparison.

* **Text:** compare if a text profile value equals, starts with, ends with, contains, (etc.) a provided number
  >Determine if the value for `email_address` ends with "@potential-customer.com"
* **Number:** compare if a numeric profile value equals, is greater than, is less than, (etc.) a provided number
  >Determine if the value for `number_of_children` is greater than 0.
* **Date:** compare if a dated numeric profile value equals, is greater than, is less than, (etc.) a provider date 
  >Determine if the value for `date_accounted_created` is prior to January 1, 2022.
* **Relative Date:** compare if a specified part of the timespan between a dated profile value and now equals, is greater than, is less than, (etc.) a provided number
  >Determine if the number of years between now and the value for `date_of_birth` is greater than 18.
* **Exists:** determine if a key does or does not exist (regardless of value)
  >Determine if a key for `account_suspended_date` exists.

These criteria can be combined to define granular Visitor Groups based on profile information.

For all criteria, the "Profile Key" value can be comma-delimited. If so, keys will be checked in order, and the first one to return a value will be used.

>Specifying a key of `dob, date_of_birth` will look for a key named `dob` and use it if it exists, if not it will look for a key called `date_of_birth`.

These profiles are intended to be ephemeral. The use case is when they're populated by some external system -- like a CDP -- on first request, then just held in a session-like state for the duration of the visitor's session, and used as a data source for Visitor Group logic so the external data source doens't have to be repeatedly queried.

The default implementation just stores the profile data in cache. If you want to change this to persist profile data, inject a new service for `IProfileStore`. (But I don't recommend it. There are better ways of doing this -- this is why CDPs exist.)

## Adding Data to a Profile

The profile can be accessed via `ProfileManager`.

```
var profileManager = ServiceLocation.Current.GetInstance<IProfileManager>();
var profile = profileManager.LoadForCurrentUser();
```

If a profile doesn't exist for this visitor, it will be automatically created and bound to their request with a persistent cookie.

You can add data to a profile manually (the `Profile` object is simply a dictionary).

```
profile["some_key"] = "some value";
profilerManager.Save(profile)
```

This should be thread-safe at the defaults. The `Profile` object is a `ConcurrentDictionary` and the default `IProfileStore` uses `MemoryCache` which is also thread-safe.

Alternately, `ProfileLoaders` can be specified in configuration which will automatically load profiles with data upon creation.

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


## Status

Unofficial, unsupported, and largely untested. It was provided to a customer as a POC.
