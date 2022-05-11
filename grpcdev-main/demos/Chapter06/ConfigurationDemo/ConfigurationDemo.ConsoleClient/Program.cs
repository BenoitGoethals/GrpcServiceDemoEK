using Grpc.Core;
using Grpc.Net.Client;
using ConfigurationDemo.ServerSide;
using Grpc.Net.Client.Configuration;

//https://docs.microsoft.com/en-us/aspnet/core/grpc/retries?view=aspnetcore-6.0
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


Greeter.GreeterClient client = new Greeter.GreeterClient(channel);

await Menu(client);

async Task SayHello(Greeter.GreeterClient client) {
    var response = await client.SayHelloAsync(new HelloRequest { Name = "Simona" });
    Console.WriteLine(response.Message);
}

async Task UploadPicture(Greeter.GreeterClient client, string filePath) {
    try {
        //the server is not going to like this because it's bigger than the MaxReceiveMessageSize of 1MB
        var request = new UploadPictureRequest();
        
        request.Picture = await Google.Protobuf.ByteString.FromStreamAsync(File.OpenRead(filePath));
        Console.WriteLine($"Sending a picture of size {request.Picture.Length / (1024 * 1024)} MB");

        var uploadReply = await client.UploadPictureAsync(request);
        Console.WriteLine($"Received a picture of size {uploadReply.Picture.Length / (1024 * 1024)} MB");
    } catch (RpcException ex) when (ex.StatusCode == StatusCode.ResourceExhausted) {
        Console.WriteLine($"PROBLEM SENDING THE IMAGE: {ex.Message}");
    }
}

async Task Menu(Greeter.GreeterClient client) {
    string choice = "";
    string filePath = "";
    Console.WriteLine("To monitor this application:");
    Console.WriteLine("Open another command prompt and install the dotnet monitor tool by typing");
    Console.WriteLine("dotnet tool install --global dotnet-counters");
    Console.WriteLine("Discover the Process Id of the client and the server by typing");
    Console.WriteLine("dotnet-counters ps");
    Console.WriteLine("To monitor the client, type (with your process id instead of 1902)");
    Console.WriteLine("dotnet-counters monitor --process-id 1902 Grpc.AspNetCore.Client");
    Console.WriteLine("To monitor the server, type (with your process id instead of 1902)");
    Console.WriteLine("dotnet-counters monitor --process-id 1902 Grpc.AspNetCore.Server");
    do {
        Console.WriteLine("1. Say Hello");
        Console.WriteLine("2. Send picture too big");
        Console.WriteLine("3. Send picture small enough");
        Console.WriteLine("q. Quit");
        choice = Console.ReadLine();
        switch (choice) {
            case "1":
                await SayHello(client);
                break;
            case "2":
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\gatto.jpg");
                await UploadPicture(client, filePath);
                break;
            case "3":
                filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\blue_flowers.jpg");
                await UploadPicture(client, filePath);
                break;
        }
    } while (!choice.ToLower().StartsWith("q"));
    Console.WriteLine("Bye.");
    

}


//https://docs.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-counters
// dotnet tool install --global dotnet-counters
// dotnet-counters ps
// dotnet-counters monitor --process-id 1902 Grpc.AspNetCore.Server
// dotnet-counters monitor --process-id 1902 Grpc.AspNetCore.Client