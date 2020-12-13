using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcServiceClientPush.Protos;

namespace GrpcServiceClientPush.Services
{
    public class ReportService:Report.ReportBase
    {
        /// <inheritdoc />
        public override async Task<Empty> AddReport(IAsyncStreamReader<Salta> requestStream, ServerCallContext context)
        {
          
            try
            {
                while (await requestStream.MoveNext() &&!context.CancellationToken.IsCancellationRequested)
                {

                    Console.WriteLine(requestStream.Current);
                 
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
             
            }

            return null;
        }
    }
}
