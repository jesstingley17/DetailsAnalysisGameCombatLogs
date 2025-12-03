using Confluent.Kafka;
using System.Text;
using System.Text.Json;

namespace CombatAnalysis.NotificationAPI.Core;

internal class JsonDocumentDeserializer : IDeserializer<JsonDocument>
{
    public JsonDocument Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        try
        {
            if (data.IsEmpty)
            {
                return null; // Or handle empty data as needed
            }

            // Assuming the data is UTF-8 encoded JSON
            string jsonString = Encoding.UTF8.GetString(data);
            return JsonDocument.Parse(jsonString);
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing JSON: {ex.Message}");
            // Handle the deserialization error appropriately
            return null; // Or throw an exception if message processing should fail
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error during deserialization: {ex.Message}");
            return null;
        }
    }
}