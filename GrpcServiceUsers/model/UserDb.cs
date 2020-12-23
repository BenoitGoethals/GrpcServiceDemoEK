using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using GrpcServiceUsers.Protos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GrpcServiceUsers.model
{
    public class UserDb
    {
        IList<User> _users = new List<User>();

        public UserDb()
        {
            Load();
        }


        private void Load()
        {

            var myJsonString = File.ReadAllText(@"c:\temp\data.json");
            _users = JsonConvert.DeserializeObject<List<User>>(myJsonString, new JsonBooleanConverter());
           

        }


        public User GetUser(string requestGuid)
        {
            var user = _users.FirstOrDefault(u => u.Guid.Equals(Guid.Parse(requestGuid)));
            return user;
        }

        public List<User> GetAllUser( bool requestActive)
        {
            return _users.Where(u => u.Active == requestActive).ToList();
        }
    }
}
