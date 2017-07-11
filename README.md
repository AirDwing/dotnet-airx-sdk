# AirX SDK For .Net



## Start

Nuget Url- https://www.nuget.org/packages/AirXSDKBase

Nuget Console:

```
Install-Package AirXSDKBase
```

CoreCli:

```
dotnet add package AirXSDKBase
```

```c#
using AirXSDKBase;

var sdk = new BaseRequest(new SDKOption {
  SecretId = "SecretId", //appid
  SecretKey = "SecretKey", //appkey
  Domain = "staging.airdwing.com", // 请求的url，
  Secure = true //是否使用ssl
});

// Get:
var result = await sdk.GET("/user/check", new { username = "18888888888" });

//POST:
var result = await sdk.POST("/org/users", new { auth = "auth*********", oid = "5555" });
```

