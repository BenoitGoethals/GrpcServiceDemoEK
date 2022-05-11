using Client;
using Test;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        services.AddHostedService<Worker>();
        services.AddSingleton<IGreetRepository, GreetRepository>();
        services.AddGrpcClient<Tester.TesterClient>(options =>
        {
            options.Address = new Uri("https://localhost:5001");
        });
    })
    .Build();

await host.RunAsync();
