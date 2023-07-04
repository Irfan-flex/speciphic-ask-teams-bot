using System.Text.Json;

namespace AskBot.Services.IO
{
    public class JsonConverter
    {
        private static readonly JsonSerializerOptions _jsonSerializerOptions;

        static JsonConverter()
        {
            _jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public static string ToString<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, _jsonSerializerOptions);
        }
    }
}
