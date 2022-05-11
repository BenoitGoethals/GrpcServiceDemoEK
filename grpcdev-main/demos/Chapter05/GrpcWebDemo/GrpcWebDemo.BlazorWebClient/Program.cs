using Grpc.Net.Client.Web;
using GrpcWebDemo.BlazorWebClient;
using GrpcWebDemo.BlazorWebClient.Core;
using GrpcWebDemo.BlazorWebClient.Core.Interfaces;
using GrpcWebDemo.ServerSide;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//https://docs.microsoft.com/en-us/aspnet/core/grpc/browser?view=aspnetcore-6.0#use-grpc-client-factory-with-grpc-web
builder.Services
    .AddGrpcClient<Greeter.GreeterClient>(options => {
        options.Address = new Uri("https://localhost:7161");
    })
    .ConfigurePrimaryHttpMessageHandler(
        () => new GrpcWebHandler(new HttpClientHandler()));

builder.Services.AddScoped<IGreeterService, GreeterService>();
await builder.Build().RunAsync();
