//https://github.com/grpc/grpc-dotnet/tree/master/examples/Transporter
using Grpc.Net.Client;
using IPCDemo.ClientSide;
using System.Net.Sockets;
using IPCDemo.ServerSide;
using Grpc.Net.Client.Configuration;
using Grpc.Core;

string SocketPath = Path.Combine(Path.GetTempPath(), "socket.tmp");

using GrpcChannel channel = CreateChannel();
Greeter.GreeterClient client = new Greeter.GreeterClient(channel);

await Unary(client);
await ClientStreaming(client);
await ServerStreaming(client);
//await Duplex(client);

Console.ReadLine();

async Task Unary(Greeter.GreeterClient client) {

    HelloReply? reply = await client.SayHelloAsync(new HelloRequest { Name = "Simona" });

    Console.WriteLine(reply.Message);

}

async Task ClientStreaming(Greeter.GreeterClient client) {
    using AsyncClientStreamingCall<HelloRequest, HelloReply>? call = client.ClientStreaming();

    await call.RequestStream.WriteAsync(new HelloRequest() { Name = "Mario" });
    await call.RequestStream.WriteAsync(new HelloRequest() { Name = "Luigi" });
    await call.RequestStream.WriteAsync(new HelloRequest() { Name = "Wario" });
    await call.RequestStream.WriteAsync(new HelloRequest() { Name = "Waluigi" });
    await call.RequestStream.CompleteAsync();
    HelloReply? response = await call;
    Console.WriteLine(response.Message);
}

async Task ServerStreaming(Greeter.GreeterClient client) {
    using AsyncServerStreamingCall<HelloReply>? call = client.ServerStreaming(new HelloRequest() { Name = "Mario" });
    await foreach (var msg in call.ResponseStream.ReadAllAsync()) {
        Console.WriteLine(msg.Message);
    }
}

async Task Duplex(Greeter.GreeterClient client) {
    using AsyncDuplexStreamingCall<HelloRequest, HelloReply>? call = client.Duplex();
    Parallel.Invoke(
        async () => {
            try {
                await call.RequestStream.WriteAsync(new HelloRequest() { Name = "Mario" });
                await call.RequestStream.WriteAsync(new HelloRequest() { Name = "Luigi" });
                await call.RequestStream.WriteAsync(new HelloRequest() { Name = "Wario" });
                await call.RequestStream.WriteAsync(new HelloRequest() { Name = "Waluigi" });
                await call.RequestStream.CompleteAsync();
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        },
        async () => {
            await foreach (var msg in call.ResponseStream.ReadAllAsync()) {
                Console.WriteLine(msg.Message);
            }
        }
    );
}
GrpcChannel CreateChannel() {
    var udsEndPoint = new UnixDomainSocketEndPoint(SocketPath);
    var connectionFactory = new UnixDomainSocketConnectionFactory(udsEndPoint);
    var socketsHttpHandler = new SocketsHttpHandler {
        ConnectCallback = connectionFactory.ConnectAsync
    };

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
    

    //http://localhost:5293;https://localhost:7293
    return GrpcChannel.ForAddress(@"http://localhost", new GrpcChannelOptions {
        HttpHandler = socketsHttpHandler,
        ServiceConfig = new ServiceConfig() { MethodConfigs = { defaultMethodConfig } }
    });
}