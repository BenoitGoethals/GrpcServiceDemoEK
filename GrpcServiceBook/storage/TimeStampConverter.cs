using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Type = System.Type;


namespace GrpcServiceBook.storage
{
    public class TimeStampConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value.ToString()?.ToLower().Trim();
            if ( objectType==typeof(Genre))
            {
                switch (value)
                {
                    case "N":
                        return Genre.Novel;
                    case "y":
                        return Genre.Romance;
                    case "S":
                        return Genre.Sfi;

                    case "W":
                        return Genre.War;
                    default:
                        return Genre.War;

                }
             
            }

            return "";

        }

        public override bool CanConvert(Type objectType)
        {
            if ( objectType == typeof(Genre))
            {
                return true;
            }
            return false;
        }
    }
}
