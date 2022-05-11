using Grpc.Core;
using GrpcWebDemo.BlazorWebClient.Core.Interfaces;
using GrpcWebDemo.ServerSide;

namespace GrpcWebDemo.BlazorWebClient.Core; 
public class GreeterService : IGreeterService {
    private readonly Greeter.GreeterClient client;

    public GreeterService(Greeter.GreeterClient client) {
        this.client = client;
    }
    public async Task<string> Greet(string name) {
        var response = await client.SayHelloAsync(new HelloRequest() { Name = name });
        return response.Message;
    }

    public async IAsyncEnumerable<string> GreetStream(string name) {
        var call = client.SayHellos(new HelloRequest() { Name = name});
        await foreach (var res in call.ResponseStream.ReadAllAsync()) { 
            yield return res.Message;
        }
    }
}
