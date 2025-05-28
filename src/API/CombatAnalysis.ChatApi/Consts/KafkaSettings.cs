using Confluent.Kafka;

namespace CombatAnalysis.ChatApi.Consts;

public class KafkaSettings
{
    public string BootstrapServers { get; set; }

    public ProducerConfig Producer { get; set; }

    public ConsumerConfig Consumer { get; set; }
}
