using System;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceClientPush.Protos;

namespace ConsoleAppGRPCClientPush
{
    public class ReportService : IReportService, IDisposable
    {
        private AsyncClientStreamingCall<Salta, Empty> call;

        private readonly GrpcChannel channelQAddress = GrpcChannel.ForAddress("https://localhost:5001");

        private Report.ReportClient cl;
        public void InitConnection()
        {

            cl = new Report.ReportClient(channelQAddress);
            call = cl.AddReport();
        }
        public async Task AddReport(Salsa salsa)
        {
          
            try
            {
                if (call?.RequestStream != null)
                    await call.RequestStream.WriteAsync(new Salta()
                    {
                        Contact = (GrpcServiceClientPush.Protos.TypeContact)salsa.contact,
                        Description = salsa.descriptio,
                        Id = salsa.Id,
                        Location = salsa.location,
                        Remarks = salsa.remark,
                        SpottedDTG = salsa.SpottedDTG.ToUniversalTime().ToTimestamp()
                    });
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
            {
                Console.WriteLine("Stream cancelled.");
            }
        }

        public async Task CloseConnectio()
        {
            await call.RequestStream.CompleteAsync();
            call.Dispose();
        }

        public void Dispose()
        {
            call?.Dispose();
        }
    }
}