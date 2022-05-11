using System;
using Grpc.Net.Client;
using Lab1;
using System.Threading.Tasks;


namespace Lab1Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using GrpcChannel grpcChannel = GrpcChannel.ForAddress("https://localhost:5001");
            HelloRequest helloRequest = new HelloRequest();
            helloRequest.Name = "Info Support";
            Greeter.GreeterClient greeterClient = new Greeter.GreeterClient(grpcChannel);
            HelloReply helloReply = greeterClient.SayHello(helloRequest);
            await grpcChannel.ShutdownAsync();
            Console.WriteLine(helloReply.Message);
        }
    }
}
