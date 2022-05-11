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

HelloReply? reply = await client.SayHelloAsync(new HelloRequest { Name = "Simona" });

Console.WriteLine(reply.Message);

Console.ReadLine();

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