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
        IList<User> Users = new List<User>();

        public UserDb()
        {
            Load();
        }


        private void Load()
        {

            var myJsonString = File.ReadAllText(@"c:\temp\data.json");
            Users = JsonConvert.DeserializeObject<List<User>>(myJsonString, new JsonBooleanConverter());
           

        }


        public User GetUser(string requestGuid)
        {
            var user = Users.FirstOrDefault(u => u.Guid.Equals(Guid.Parse(requestGuid)));
            return user;
        }

        public List<User> GetAllUser( bool requestActive)
        {
            return Users.Where(u => u.Active == requestActive).ToList();
        }
    }



    public class User
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string ForName { get; set; }
        public string LatName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }

    }
    public class JsonBooleanConverter : JsonConverter
    {
        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString().ToLower().Trim();
            switch (value)
            {
                case "true":
                case "yes":
                case "y":
                case "1":
                    return true;
            }
            return false;
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(Boolean))
            {
                return true;
            }
            return false;
        }
    }
}
