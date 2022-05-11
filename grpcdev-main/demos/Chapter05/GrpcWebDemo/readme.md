# GrpcWeb

In this demo, we show how to implement a gRpc Service that can accept calls from a web client.  
We also have a web client implemented in Blazor. The calls originate from the WebAssembly code situated on the client browser.  

Server side, we can follow the instructions described in //https://docs.microsoft.com/en-us/aspnet/core/grpc/browser?view=aspnetcore-6.0

We need to configure CORS to accept calls from the client.
```cs
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));
```

We can now accept calls fro a web client by adding and configuring the correct middleware:

```cs
app.UseGrpcWeb();
app.MapGrpcService<GreeterService>().EnableGrpcWeb().RequireCors("AllowAll"); 
```

Client side we use a client factory integration to create a client, as described on https://docs.microsoft.com/en-us/aspnet/core/grpc/browser?view=aspnetcore-6.0#use-grpc-client-factory-with-grpc-web

```cs
builder.Services
    .AddGrpcClient<Greeter.GreeterClient>(options => {
        options.Address = new Uri("https://localhost:7161");
    })
    .ConfigurePrimaryHttpMessageHandler(
        () => new GrpcWebHandler(new HttpClientHandler()));
```

We register a service in the DI container:

```cs
builder.Services.AddScoped<IGreeterService, GreeterService>();
```

We can now implement our service, which in turn uses our GrpcClient:

```cs
using Grpc.Core;
using GrpcWebDemo.ServerSide;

namespace GrpcWebDemo.ServerSide.Services {
    public class GreeterService : Greeter.GreeterBase {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger) {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) {
            return Task.FromResult(new HelloReply {
                Message = "Hello " + request.Name
            });
        }
        public override async Task SayHellos(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context) {
            await responseStream.WriteAsync(new HelloReply() { Message = "Hello " + request.Name });
            await Task.Delay(1000);
            await responseStream.WriteAsync(new HelloReply() { Message = "Nice to"});
            await Task.Delay(1000);
            await responseStream.WriteAsync(new HelloReply() { Message = "meet you!" });
        }
    }
}
```

- The `Greetings` razor component uses the GreeterService to display the greetings, demonstrating a Unary Call.  
- The `GreetingsStream` razor component uses the GreeterService to display the stream of greetings, demonstrating a Server Streaming Call.

Client Streaming and Duplex are not supported.


