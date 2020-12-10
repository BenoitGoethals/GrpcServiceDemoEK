using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcServiceDemoEK;

namespace StarterConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            Console.WriteLine("Hello GRPC!");
            var client = new Greeter.GreeterClient(channel);
            var response = await client.SayHelloAsync(new HelloRequest() {Name = "test"});

            Console.WriteLine("Greeting: " + response.Message);

            Console.ReadLine();
        }
    }
}
