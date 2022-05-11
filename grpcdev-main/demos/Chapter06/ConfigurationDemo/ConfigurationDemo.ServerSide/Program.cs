using ConfigurationDemo.ServerSide.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc(options => { 
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = 1024 * 1024 * 1;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

//Our server starts very slowly. Without a retry policy the client will get an exception.
await Task.Delay(3000);
app.Run();
