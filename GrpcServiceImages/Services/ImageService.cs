using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcServiceImages.Protos;

namespace GrpcServiceImages.Services
{
    public class ImageService : Images.ImagesBase
    {
        public override async Task<status> addImage(IAsyncStreamReader<UploadFileRequest> requestStream, ServerCallContext context)
        {

            string nameOfFile = "Unknown.jpg";
            long size = 0;
            await requestStream.MoveNext();
            if (requestStream.Current.Info != null)
            {
                nameOfFile = requestStream.Current.Info.Name;
                size = requestStream.Current.Info.Size;
            }

            List<Byte> bytes = new List<byte>();
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {

                var b = requestStream.Current.ChunkData;
                bytes.Add(Convert.ToByte(b.ToStringUtf8()));


            }

            if (size == bytes.Count)
            {
                await using (var ms = new MemoryStream(bytes.ToArray()))
                {
                    await using (var fs = new FileStream(path: $"c:/temp/{nameOfFile}", FileMode.Create))
                    {
                        
                        ms.WriteTo(fs);
                    }
                }


                return new status() { Msg = true };
            }

            return new status() { Msg = false };
        }


        public override async Task<status> addImageBulk(IAsyncStreamReader<UploadFileRequest> requestStream,
            ServerCallContext context)
        {
           
            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                string nameOfFile = "Unknown.jpg";
                long size = 0;
              
                if (requestStream.Current.Info != null)
                {
                    nameOfFile = requestStream.Current.Info.Name;
                    size = requestStream.Current.Info.Size;
                    List<Byte> bytes = new List<byte>();
                    while (bytes.Count != size && await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested )
                    {

                        var b = requestStream.Current.ChunkData;
                        bytes.Add(Convert.ToByte(b.ToStringUtf8()));


                    }

                    if (size == bytes.Count)
                    {
                        await using (var ms = new MemoryStream(bytes.ToArray()))
                        {
                            await using (var fs = new FileStream(path: $"c:/temp/{nameOfFile}", FileMode.Create))
                            {
                                ms.WriteTo(fs);
                            }
                        }


                     
                    }
                }

               
            }
            return new status() { Msg = true };
        }
    }
}
