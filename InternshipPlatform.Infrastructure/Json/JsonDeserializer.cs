using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace InternshipPlatform.Infrastructure.Json
{
    public class JsonDeserializer<T> : IDeserializer<T> where T : class, new()
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (isNull || data.IsEmpty)
                return new T();

            return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(data));
        }
    }
}
