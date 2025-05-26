using CombatAnalysis.ChatApi.Interfaces;
using Confluent.Kafka;

namespace CombatAnalysis.ChatApi.Services;

internal class KafkaProducerService<TKey, TValue> : IKafkaProducerService<TKey, TValue>
{
    private readonly IProducer<TKey, TValue> _producer;
    private readonly ILogger<KafkaProducerService<TKey, TValue>> _logger;

    public KafkaProducerService(IConfiguration configuration, ILogger<KafkaProducerService<TKey, TValue>> logger)
    {
        var producerConfig = configuration.GetSection("Kafka:Producer").Get<ProducerConfig>() ?? new ProducerConfig();
        producerConfig.BootstrapServers ??= configuration.GetSection("Kafka:BootstrapServers").Value;

        if (string.IsNullOrEmpty(producerConfig.BootstrapServers))
        {
            throw new ArgumentException("Kafka BootstrapServers configuration is missing or invalid.");
        }

        _producer = new ProducerBuilder<TKey, TValue>(producerConfig).Build();
        _logger = logger;
    }

    public async Task ProduceAsync(string topic, TKey key, TValue value)
    {
        try
        {
            var message = new Message<TKey, TValue> { Key = key, Value = value };
            var deliveryResult = await _producer.ProduceAsync(topic, message);

            _logger.LogInformation($"Message delivered to '{topic}' - Partition: {deliveryResult.Partition}, Offset: {deliveryResult.Offset}");
        }
        catch (ProduceException<TKey, TValue> ex)
        {
            _logger.LogError($"Failed to deliver message to '{topic}': {ex.Error.Reason}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while producing to '{topic}': {ex.Message}");
        }
    }

    public async Task ProduceAsync(string topic, Message<TKey, TValue> message)
    {
        try
        {
            var deliveryResult = await _producer.ProduceAsync(topic, message);
            _logger.LogInformation($"Message delivered to '{topic}' - Partition: {deliveryResult.Partition}, Offset: {deliveryResult.Offset}");
        }
        catch (ProduceException<TKey, TValue> ex)
        {
            _logger.LogError($"Failed to deliver message to '{topic}': {ex.Error.Reason}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"An unexpected error occurred while producing to '{topic}': {ex.Message}");
        }
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}
