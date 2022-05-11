# Configuration and Monitoring

In this demo we show how to configure and monitor the gRpc service.

Server side, we do not accept messages bigger than 1MB. We do that by configuring the server:

```cs
builder.Services.AddGrpc(options => { 
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = 1024 * 1024 * 1;
});
```

When the client tries to send an image larger than 1MB, it gets an RpcException with a status code of `StatusCode.ResourceExhausted`.



As explained in https://docs.microsoft.com/en-us/aspnet/core/grpc/retries?view=aspnetcore-6.0 on the client we configure a retry policy to retry failed calls.


```cs
var defaultMethodConfig = new MethodConfig {
    Names = { MethodName.Default },
    RetryPolicy = new RetryPolicy {
        MaxAttempts = 5,
        InitialBackoff = TimeSpan.FromSeconds(1),
        MaxBackoff = TimeSpan.FromSeconds(5),
        BackoffMultiplier = 1.5,
        RetryableStatusCodes = { StatusCode.Unavailable }
    }
};
GrpcChannelOptions options = new GrpcChannelOptions() {
    //MaxRetryAttempts = 5,
    ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } }
};


//Our server starts very slowly. Without a retry policy we'll get an exception.
GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7116" , options );
```

To monitor this application  
Open another command prompt and install the dotnet monitor tool by typing  

```powershell
dotnet tool install --global dotnet-counters
```

Discover the Process Id of the client and the server by typing

```powershell
dotnet-counters ps
```

To monitor the client, type (with your process id instead of 1902)

```powershell
dotnet-counters monitor --process-id 1902 Grpc.AspNetCore.Client
```

To monitor the server, type (with your process id instead of 1902)

```powershell
dotnet-counters monitor --process-id 1902 Grpc.AspNetCore.Server
```