//https://github.com/grpc/grpc-dotnet/tree/master/examples/Transporter
using IPCDemo.ServerSide.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

string SocketPath = Path.Combine(Path.GetTempPath(), "socket.tmp");

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();


builder.WebHost.UseKestrel(options =>
{
    if (File.Exists(SocketPath)) {
        File.Delete(SocketPath);
    }
    options.ListenUnixSocket(SocketPath, listenOptions =>
    {
        //listenOptions.Protocols = HttpProtocols.Http2;
        //listenOptions.UseHttps();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
