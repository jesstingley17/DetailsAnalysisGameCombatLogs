using CombatAnalysis.ChatApi.Core;
using Confluent.Kafka;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Services;

public abstract class KafkaConsumerServiceBase : BackgroundService
{
    private readonly ConsumerConfig _config;
    private readonly string _topic;
    private readonly ILogger _logger;
    private IConsumer<string, JsonDocument>? _consumer;

    protected KafkaConsumerServiceBase(IConfiguration configuration, string topic, ILogger logger, string configSection = "Kafka:Consumer")
    {
        _config = configuration.GetSection(configSection).Get<ConsumerConfig>() ?? new ConsumerConfig();
        _config.BootstrapServers = _config.BootstrapServers ?? configuration.GetSection("Kafka:BootstrapServers").Value;

        if (string.IsNullOrEmpty(_config.BootstrapServers) || string.IsNullOrEmpty(_config.GroupId))
        {
            throw new ArgumentException("Kafka BootstrapServers or GroupId configuration is missing or invalid.");
        }

        _topic = topic ?? throw new ArgumentNullException(nameof(topic));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected abstract Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> result, CancellationToken stoppingToken);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerConfig = new ConsumerConfig(_config);
        _consumer = new ConsumerBuilder<string, JsonDocument>(consumerConfig)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(new JsonDocumentDeserializer())
            .SetErrorHandler((_, e) => _logger.LogError($"Kafka Consumer Error: {e.Reason}"))
            .SetStatisticsHandler((_, json) => _logger.LogDebug($"Kafka Statistics: {json}"))
            .Build();

        _consumer.Subscribe(_topic);
        _logger.LogInformation($"Kafka consumer subscribed to topic '{_topic}' with GroupId '{_config.GroupId}'.");

        await Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(5)); // Adjust timeout as needed

                    if (consumeResult?.Message != null)
                    {
                        _logger.LogDebug($"Received message from '{_topic}' - Partition: {consumeResult.Partition}, Offset: {consumeResult.Offset}");
                        await ConsumeMessageAsync(consumeResult, stoppingToken);
                        // Optionally commit the offset manually if auto-commit is disabled
                        _consumer.Commit(consumeResult);
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError($"Consume error on topic '{_topic}': {ex.Error.Reason}");
                    // Consider implementing retry or dead-letter queue logic
                }
                catch (OperationCanceledException)
                {
                    // Expected when the service is stopping
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An unexpected error occurred while consuming from '{_topic}': {ex.Message}");
                    // Handle other exceptions
                }
            }

            _consumer.Close();
            _logger.LogInformation($"Kafka consumer for topic '{_topic}' stopped.");
        }, stoppingToken);
    }

    public override void Dispose()
    {
        _consumer?.Dispose();
        base.Dispose();
    }
}
