using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcServiceImages.Protos;

namespace ConsoleAppImage
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello send!");
            try
            {
              CancellationTokenSource cancellationTokenSource=new CancellationTokenSource();  
              var stream=  File.ReadAllBytes("grpc-basics.png");
             
              var channel = GrpcChannel.ForAddress("https://localhost:5001");
              Images.ImagesClient client=new Images.ImagesClient(channel);
              //var call = client.addImage();
              //await call.RequestStream
              //    .WriteAsync(new UploadFileRequest() { Info=new GrpcServiceImages.Protos.FileInfo(){Name = "c:/temp/grpc-basics.png",Size = stream.Length } });
              //  foreach (var b in (stream))
              //{
              //    await call.RequestStream
              //        .WriteAsync(new UploadFileRequest() {ChunkData = ByteString.CopyFromUtf8(b.ToString())});

              //}




              //await call.RequestStream.CompleteAsync();
              //var resp = await call;
              //Console.WriteLine(resp.Msg);



             var call = client.addImageBulk();
              await call.RequestStream
                  .WriteAsync(new UploadFileRequest() { Info = new GrpcServiceImages.Protos.FileInfo() { Name = "grpc-basics.png", Size = stream.Length } });
              foreach (var b in (stream))
              {
                  await call.RequestStream
                      .WriteAsync(new UploadFileRequest() { ChunkData = ByteString.CopyFromUtf8(b.ToString()) });

              }
              await call.RequestStream
                  .WriteAsync(new UploadFileRequest() { Info = new GrpcServiceImages.Protos.FileInfo() { Name = "grpc-basics2.png", Size = stream.Length } });
              foreach (var b in (stream))
              {
                  await call.RequestStream
                      .WriteAsync(new UploadFileRequest() { ChunkData = ByteString.CopyFromUtf8(b.ToString()) });

              }



                await call.RequestStream.CompleteAsync();
               
            var   resp = await call;
              Console.WriteLine(resp.Msg);













            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Console.WriteLine("send");
            Console.ReadLine();
        }
    }
}
