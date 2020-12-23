using System;
using System.Collections.Concurrent;
using System.Linq;
using GrpcServiceChatter.Services;

namespace GrpcServiceChatter.model
{
    public class ChatCloud
    {
        readonly ConcurrentBag<ChatService> _chatServices = new ConcurrentBag<ChatService>();
        public Guid Guid { get; set; } = new Guid();
        public string Name { get; set; }

        private readonly ConcurrentBag<Msg> _msgs = new ConcurrentBag<Msg>();


        public void AddMsg(Msg msg)
        {
            _msgs.Add(msg);
            foreach (var chatService in _chatServices)
            {
                chatService.Notify(msg);
                Console.WriteLine("notify :" + chatService.Guid);
            }
        }

        public void Subscribe(ChatService observer)
        {
            if (!_chatServices.Contains(observer))
            {
                _chatServices.Add(observer);
            }

        }
    }
}