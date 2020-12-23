using System.Collections.Generic;
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
}
