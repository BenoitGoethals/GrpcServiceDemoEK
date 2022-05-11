# Client Factory Integration

In this example, we show how to use a gRpc Client Factory, as described on the [Client Factory](https://docs.microsoft.com/en-us/aspnet/core/grpc/clientfactory?view=aspnetcore-6.0) page.

- The server is a simple greeter service that greets the client.
- The client is a Razor Pages application that uses the Client Factory to create a gRpc client.

On the client, the Greet Razor Page gets the client injected in its constructor and uses it without having to create it explicitly. We also don't need to create the channel manually.  
This is because we have added the `Google.Net.ClientFactory` NuGet package to the project and we have added GrpcClient as a service to the service container of the web application in the Program.cs file:

```cs
builder.Services.AddGrpcClient<Greeter.GreeterClient>(o => {
    o.Address = new Uri(builder.Configuration["GrpcAddress"]);
});
```
