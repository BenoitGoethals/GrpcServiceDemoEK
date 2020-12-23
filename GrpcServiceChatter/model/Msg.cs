using System;

namespace GrpcServiceChatter.model
{
    public class Msg
    {
        public Guid Guid { get; set; }
        public string Content { get; set; }

        public string Chatroom { get; set; }
        public string Chatter { get; set; }
    }
}