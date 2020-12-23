using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Common.model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Genre
    {
        SFI,
        WAR,
        Romance,
        Novel
    }
}