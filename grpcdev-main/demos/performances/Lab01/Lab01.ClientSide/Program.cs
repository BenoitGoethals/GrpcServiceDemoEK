using Grpc.Core;
using Grpc.Net.Client;
using Lab01.ServerSide;

using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7164");
HelloRequest request = new HelloRequest() { Name = "Simona" };
Greeter.GreeterClient client = new Greeter.GreeterClient(channel);

await Unary(client);
await ClientStreaming(client);
await ServerStreaming(client);
//await Duplex(client);

Console.ReadLine();
await channel.ShutdownAsync();


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