using System;
using Grpc.Net.Client;
using Lab4;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http; 

namespace Lab4Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            x509Store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection myCollection = x509Store.Certificates.Find(X509FindType.FindBySubjectName, "Your Name", true);
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificates.Add(myCollection[0]);
            GrpcChannelOptions grpcChannelOptions = new GrpcChannelOptions { HttpHandler = httpClientHandler };
            using GrpcChannel grpcChannel = GrpcChannel.ForAddress("https://localhost:5001", grpcChannelOptions);
            Greeter.GreeterClient greeterClient = new Greeter.GreeterClient(grpcChannel);
            HelloRequest request = new HelloRequest{ Name = "Info Support"};
            HelloReply reply = greeterClient.SayHello(request);
            Console.WriteLine(reply.Message);
            await grpcChannel.ShutdownAsync();
        }
    }
}
