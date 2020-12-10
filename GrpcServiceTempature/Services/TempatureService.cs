using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServiceTempature.Protos;
using Microsoft.Extensions.Logging;

namespace GrpcServiceTempature.Services
{
    public class TempatureService:Climate.ClimateBase
    {
        private readonly ILogger<TempatureService> _logger;

        public TempatureService(ILogger<TempatureService> logger)
        {
            _logger = logger;
        }


        public override async Task GetTemperature(Protos.HelloRequest request, IServerStreamWriter<TempatureReply> responseStream, ServerCallContext context)
        {
            var rng = new Random();
            var now = DateTime.UtcNow;

         ;
            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500); // Gotta look busy

                var forecast = rng.Next(0, 100);

                _logger.LogInformation("Sending WeatherData response");

                await responseStream.WriteAsync(new TempatureReply(){Message=forecast});
            }
        }
    }
}
