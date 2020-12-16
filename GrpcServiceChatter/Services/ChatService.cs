using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcServiceChatter.model;
using Microsoft.Extensions.Logging;

namespace GrpcServiceChatter.Services
{
    public class ChatService : GrpcServiceChatter.ChatRoomService.ChatRoomServiceBase
    {
        public Guid Guid=Guid.NewGuid();
        private readonly ILogger<ChatService> _logger;
        private readonly ChatRoomManager _chatRoomManager;

        private readonly ConcurrentQueue<Msg> msgs = new ConcurrentQueue<Msg>();

        public ChatService(ILogger<ChatService> logger, ChatRoomManager chatRoomManager)
        {
            _logger = logger;
            this._chatRoomManager = chatRoomManager;
        }

        public override async Task SendMessage(IAsyncStreamReader<MsgChat> requestStream, IServerStreamWriter<MsgChat> responseStream, ServerCallContext context)
        {

            var httpContext = context.GetHttpContext();
            _logger.LogInformation($"Connection id: {httpContext.Connection.Id}");


            if (!await requestStream.MoveNext())
            {
                return;
            }



            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                var note = requestStream.Current;

                _chatRoomManager.AddMsg(note.Chatroon, new Msg() { Chatter = note.Chatter, Guid = Guid.Parse(note.Id), Content = note.Msg, Chatroom = note.Chatroon }, this);
                if (msgs.TryDequeue(out var msg))
                {
                    await responseStream.WriteAsync(new MsgChat()
                    {
                        Id = msg.Guid.ToString(),
                        Chatter = msg.Chatter,
                        Msg = msg.Content,
                        Chatroon = msg.Chatroom
                    });
                }

            }
         




            //   await responseStream.WriteAsync();

        }
        public void Notify(Msg msg)
        {
            lock (msg)
            {
                msgs.Enqueue(msg);
            }
           

        }
    }
}
