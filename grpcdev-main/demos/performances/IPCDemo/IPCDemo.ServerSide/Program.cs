//https://github.com/grpc/grpc-dotnet/tree/master/examples/Transporter

//https://ghz.sh/docs/usage
//ghz --insecure --call greet.Greeter.SayHello -d "{\"name\":\"Simona\"}" "unix:C:\\Users\\SimonaC\\AppData\\Local\\Temp\\socket.tmp" -c 10 -n 10000 --rps 200
//ghz --insecure --call greet.Greeter.ClientStreaming -d "[{\"name\":\"Simona\"},{\"name\":\"Mario\"}]" "unix:C:\\Users\\SimonaC\\AppData\\Local\\Temp\\socket.tmp" -c 10 -n 10000 --rps 200

using IPCDemo.ServerSide.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

string SocketPath = Path.Combine(Path.GetTempPath(), "socket.tmp");

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.WebHost.UseKestrel(options =>
{
    if (File.Exists(SocketPath)) {
        File.Delete(SocketPath);
    }
    options.ListenUnixSocket(SocketPath, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
        //listenOptions.UseHttps();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcReflectionService();
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
