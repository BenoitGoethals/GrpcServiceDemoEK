using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Lab3
{
    public class CalculateService : Calculate.CalculateBase
    {
        private readonly ILogger<CalculateService> _logger;
        public CalculateService(ILogger<CalculateService> logger)
        {
            _logger = logger;
        }

        public async override Task AddAndFactor(IAsyncStreamReader<CalcRequest> requestStream, IServerStreamWriter<CalcReply> responseStream, ServerCallContext context)
        {
            uint sum = 0;
            await foreach (CalcRequest cr in requestStream.ReadAllAsync())
                sum += cr.Number;
            CalcReply reply = new CalcReply();
            uint i = 2;
            while (sum > 1)
            {
                while (sum % i == 0)
                {
                    reply.Factor = i;
                    await responseStream.WriteAsync(reply);
                    sum /= i;
                }
                i = (i == 2) ? 3 : i + 2;
            }
        }
    }
}
