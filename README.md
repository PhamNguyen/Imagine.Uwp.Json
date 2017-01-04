# Json Parse for Universal Windows Platform

Library for deserialize json string to c# object.

## nuget
```
Install-Package Imagine.Uwp.Json
```
#public static T Json.Parse<T>(jsonString)
```

T: c# class or type.
jsonString: json string for deserialize to object

Sample:
// Setup deserialize property by Sign("property")
public class User{
         [Sign("id")]
         public string Id {set; get;}

         [Sign("name")]
         public string Name{set; get;}
}
// Request user data 
String _data = restClient.RequestStringAsync();

// deserialize by Json.Parse
var user = Json.Parse<User>(_data);
