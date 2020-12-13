using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Grpc.Core.Logging;

namespace ConsoleAppGRPCClientPush
{
    class Program
    {
        static void Main(string[] args)
        {
         
            Console.WriteLine("GRPC Client Streaming");
            IReportService reportService=new ReportService();
            reportService.InitConnection();
            _ = Task.Run(function: async () =>
            {
                try
                {

                    for (int i = 0; i < 10000; i++)
                    {
                        var sl = new Salsa()
                        {
                            contact = TypeContact.Aie, descriptio = "trst" + i, Id = i, location = "10",
                            remark = "dsfdsf",
                            SpottedDTG = DateTime.Now
                        };
                        await reportService.AddReport(sl);
                    //
                    // await Task.Delay(TimeSpan.FromSeconds(1));
                        Console.WriteLine(sl.Id);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);


                }
            });
     //       reportService.CloseConnectio();

            Console.ReadLine();

        }
    }
}
