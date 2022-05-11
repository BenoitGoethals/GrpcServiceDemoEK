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

        
        public async override Task<CalcReply> Add(IAsyncStreamReader<CalcRequest> requestStream, ServerCallContext context)
        {
            uint sum = 0;
            await foreach (CalcRequest cr in requestStream.ReadAllAsync())
                sum += cr.Number;
            return new CalcReply { Result = sum };
        }
    }
}
