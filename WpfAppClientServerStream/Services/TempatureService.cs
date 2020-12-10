using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceTempature.Protos;
using Unity.Lifetime;

namespace WpfAppClientServerStream.Services
{
    public interface ITempatureService
    {
        event EventHandler<int> TempatureChange;
        Task StartProcess();
    }

    public class TempatureService : ITempatureService
    {
       
        public event EventHandler<int> TempatureChange ;


        public async Task StartProcess()
        {
          
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Climate.ClimateClient(channel);

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(50000));
            using var streamingCall = client.GetTemperature(new HelloRequest(){City="gent"}, cancellationToken: cts.Token);

            try
            {
                await foreach (var weatherData in streamingCall.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
                {
                    if (weatherData.Message == 70)
                    {
                        Console.WriteLine("bingo 70");
                        cts.Cancel();
                        break;
                    }
                    OnTempatureChange(weatherData.Message);
                }
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Stream cancelled.");
            }


        }

        protected virtual void OnTempatureChange(int temp) //protected virtual method
        {
        
            TempatureChange?.Invoke(this, temp);
        }

     
    }
}
