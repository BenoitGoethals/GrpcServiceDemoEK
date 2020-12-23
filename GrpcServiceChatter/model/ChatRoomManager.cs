using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatCommon.model;
using GrpcServiceChatter.Services;

namespace GrpcServiceChatter.model
{
    public class ChatRoomManager
    {
        private readonly IDictionary<string, ChatCloud> _chatRooms = new Dictionary<string, ChatCloud>();

        public void AddMsg(string chatcloud, Msg msg, ChatService chatService)
        {
            if (!_chatRooms.ContainsKey(chatcloud))
            {
                _chatRooms.Add(chatcloud, new ChatCloud() { Name = chatcloud });

            }
            _chatRooms[chatcloud].Subscribe(chatService);
            _chatRooms[chatcloud].AddMsg(msg);

        }
    }


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
            //  Console.WriteLine(observer.Guid);
            if (!_chatServices.Contains(observer))
            {
                _chatServices.Add(observer);
            }

        }
    }

    public class Msg
    {
        public Guid Guid { get; set; }
        public string Content { get; set; }

        public string Chatroom { get; set; }
        public string Chatter { get; set; }
    }



}
