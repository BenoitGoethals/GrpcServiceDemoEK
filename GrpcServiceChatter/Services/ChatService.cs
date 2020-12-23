using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcServiceChatter.model;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.Extensions.Logging;

namespace GrpcServiceChatter.Services
{
    public class ChatService : GrpcServiceChatter.ChatRoomService.ChatRoomServiceBase, IDisposable
    {
        public Guid Guid=Guid.NewGuid();
        private readonly ILogger<ChatService> _logger;
        private readonly ChatRoomManager _chatRoomManager;

        private readonly ConcurrentQueue<Msg> _msgs = new ConcurrentQueue<Msg>();

        private  Task _watcher;

        public ChatService(ILogger<ChatService> logger, ChatRoomManager chatRoomManager)
        {
            _logger = logger;
            this._chatRoomManager = chatRoomManager;
        }

        public override async Task SendMessage(IAsyncStreamReader<MsgChat> requestStream, IServerStreamWriter<MsgChat> responseStream, ServerCallContext context)
        {

            var httpContext = context.GetHttpContext();
            _logger.LogInformation($"Connection id: {httpContext.Connection.Id}");

            _watcher ??= new Task(async () =>
            {
                while (!context.CancellationToken.IsCancellationRequested)
                {
                    if (_msgs.TryDequeue(out Msg msg))
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
                

            });

            _watcher.Start();


            while (await requestStream.MoveNext() && !context.CancellationToken.IsCancellationRequested)
            {
                var note = requestStream.Current;

                
                _chatRoomManager.AddMsg(note.Chatroon, new Msg() { Chatter = note.Chatter, Guid = Guid.Parse(note.Id), Content = note.Msg, Chatroom = note.Chatroon }, this);
                

            }
         


        }
        public void Notify(Msg msg)
        {
           
                _msgs.Enqueue(msg);

        }

        public void Dispose()
        {
            _watcher?.Dispose();
        }
    }
}
