using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mv.Modules.P99.Service
{
    public partial class CheckData
    {
        [JsonProperty("SN")]
        public string Sn { get; set; }

        [JsonProperty("Station")]
        public string Station { get; set; }

        [JsonProperty("Project")]
        public string Project { get; set; } = "LTBWhiPlash";
    }
    public partial class CheckData
    {
        public static CheckData FromJson(string json) => JsonConvert.DeserializeObject<CheckData>(json, Converter.Settings);
    }
    public partial class UploadData
    {
        [JsonProperty("Time")]
        public DateTimeOffset Time { get; set; } = new DateTimeOffset(DateTime.Now);

        [JsonProperty("SN")]
        public string Sn { get; set; }

        [JsonProperty("SpindleNO")]
        public string SpindleNo { get; set; }

        [JsonProperty("MandrelNO")]
        public string MandrelNo { get; set; }

        [JsonProperty("Result")]
        public string Result { get; set; }

        [JsonProperty("Line")]
        public string Line { get; set; }

        [JsonProperty("Station")]
        public string Station { get; set; }

        [JsonProperty("Project")]
        public string Project { get; set; }
    }
    public partial class UploadData
    {
        public static UploadData FromJson(string json) => JsonConvert.DeserializeObject<UploadData>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this UploadData self) => JsonConvert.SerializeObject(self, Converter.Settings);
        public static string ToJson(this CheckData self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }


    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeFormat="yyyy-MM-dd HH:mm:ss" }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }
        //LTBWhiPlash
        //CoilWinding
        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
