using System;
using Grpc.Net.Client;
using System.Linq;
using Grpc.Core;
using System.Threading.Tasks;
using Lab3;

namespace CalcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcChannel grpcChannel = GrpcChannel.ForAddress("https://localhost:5001");
            CalcRequest request = new CalcRequest();
            Calculate.CalculateClient calculateClient = new Calculate.CalculateClient(grpcChannel);
            AsyncClientStreamingCall<CalcRequest,CalcReply> proxy = calculateClient.Add();
            IClientStreamWriter<CalcRequest> writer = proxy.RequestStream;
            foreach (int n in Enumerable.Range(0, 100))
            {
                request.Number = (uint)n;
                await writer.WriteAsync(request);
            }
            await writer.CompleteAsync();
            CalcReply reply = await proxy.ResponseAsync;
            Console.WriteLine($"The result is: {reply.Result}");
            proxy.Dispose();
        }
    }
}
