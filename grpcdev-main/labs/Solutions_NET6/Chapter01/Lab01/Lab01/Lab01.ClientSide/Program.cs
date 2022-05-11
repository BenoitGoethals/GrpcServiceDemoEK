using Grpc.Net.Client;
using Lab01.ServerSide;

using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7164");
HelloRequest request = new HelloRequest() { Name = "Simona" };
Greeter.GreeterClient client = new Greeter.GreeterClient(channel);
HelloReply reply = await client.SayHelloAsync(request);
Console.WriteLine(reply.Message);
Console.ReadLine();
await channel.ShutdownAsync();