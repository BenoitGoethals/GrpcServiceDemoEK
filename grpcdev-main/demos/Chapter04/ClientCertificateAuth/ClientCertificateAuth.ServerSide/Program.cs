using ClientCertificateAuth.ServerSide.Services;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

//https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-6.0#configure-your-server-to-require-certificates
builder.Services.Configure<KestrelServerOptions>(options => {
    options.ConfigureHttpsDefaults(options =>
        options.ClientCertificateMode = ClientCertificateMode.RequireCertificate);
});

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

//https://docs.microsoft.com/en-us/aspnet/core/security/authentication/certauth?view=aspnetcore-6.0
builder.Services
    .AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate(o => o.RevocationMode = X509RevocationMode.NoCheck);

var app = builder.Build();

app.UseAuthentication();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
