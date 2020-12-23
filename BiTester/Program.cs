using System;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceChatter;

namespace BiTester
{
    class Program
    {
        public static  void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //var httpHandler = new HttpClientHandler();
            //// Return `true` to allow certificates that are untrusted/invalid
            //httpHandler.ServerCertificateCustomValidationCallback =
            //HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _ = Task.Run(async () =>
              {


                  GrpcChannel channelQAddress = GrpcChannel.ForAddress("https://localhost:5001");
                  var client = new GrpcServiceChatter.ChatRoomService.ChatRoomServiceClient(channelQAddress);
                  using var call = client.SendMessage();
                  var responseReaderTask = Task.Run(async () =>
                  {
                      while (await call.ResponseStream.MoveNext())
                      {
                          var note = call.ResponseStream.Current;
                         Console.WriteLine("1 Received " + note);
                      }
                  });

                  while (true)
                  {
                     await Task.Delay(TimeSpan.FromSeconds(1));
                      await call.RequestStream.WriteAsync(new MsgChat() { Chatroon = "1", Chatter = "1", Id = Guid.NewGuid().ToString(), Msg = "fdFDSFds" });
                  }
              });
            _ = Task.Run(async () =>
            {


                GrpcChannel channelQAddress = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new GrpcServiceChatter.ChatRoomService.ChatRoomServiceClient(channelQAddress);
                using (var call = client.SendMessage())
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var note = call.ResponseStream.Current;
                 //          Console.WriteLine("2 Received " + note);
                        }
                    });

                    while (true)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(3));
                        await call.RequestStream.WriteAsync(new MsgChat() { Chatroon = "2", Chatter = "2dfdsf", Id = Guid.NewGuid().ToString(), Msg = "fdFDSFds" });
                    }

                }



            });

            _ = Task.Run(async () =>
            {


                GrpcChannel channelQAddress = GrpcChannel.ForAddress("https://localhost:5001");
                var client = new GrpcServiceChatter.ChatRoomService.ChatRoomServiceClient(channelQAddress);
                using (var call = client.SendMessage())
                {
                    var responseReaderTask = Task.Run(async () =>
                    {
                        while (await call.ResponseStream.MoveNext())
                        {
                            var note = call.ResponseStream.Current;
                   //         Console.WriteLine("Received " + note);
                        }
                    });

                    while (true)
                    {
                       await Task.Delay(TimeSpan.FromSeconds(1));
                        await call.RequestStream.WriteAsync(new MsgChat() { Chatroon = "3", Chatter = "3dfdsf", Id = Guid.NewGuid().ToString(), Msg = "fdFDSFds" });
                    }

                }



            });


            Console.ReadLine();
        }
    }
}

