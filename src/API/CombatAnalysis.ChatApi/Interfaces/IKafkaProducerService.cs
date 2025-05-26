using Confluent.Kafka;

namespace CombatAnalysis.ChatApi.Interfaces;

public interface IKafkaProducerService<TKey, TValue> : IDisposable
{
    Task ProduceAsync(string topic, TKey key, TValue value);

    Task ProduceAsync(string topic, Message<TKey, TValue> message);
}
