using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatCommon.model;

namespace GrpcServiceChatter.model
{
    public class ChatRoomManager
    {
        private readonly IDictionary<string, ChatRoom> _chatRooms = new Dictionary<string, ChatRoom>();

        public void AddChatRoom(ChatCommon.model.ChatRoom chatRoom)
        {
            this._chatRooms.Add(chatRoom.Name,chatRoom);
        }

        public ChatRoom GetChatRoom(string chatRoomName)
        {
            return _chatRooms[chatRoomName];
        }


    }
}
