# AirX SDK For .Net



## Start

Nuget Url- https://www.nuget.org/packages/Dwing.AirXSDKBase

Nuget Console:

```
Install-Package Dwing.AirXSDKBase
```

CoreCli:

```
dotnet add package Dwing.AirXSDKBase
```

```c#
using AirXBase;

var sdk = new AirXBase(new AirXBaseOption 
{
  SecretId = "SecretId", //appid
  SecretKey = "SecretKey", //appkey
  Domain = "staging.airdwing.com", // 请求的url，
  Secure = true //是否使用ssl
});

// Get:
var result = await sdk.GET("/user/check", new { username = "18888888888" });

// POST:
var result = await sdk.POST("/org/users", new { auth = "auth*********", oid = "5555" });
```

