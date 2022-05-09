using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using CustomerManager.Interfaces;
using CustomerManager.Model;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace CustomerManager.Repository;

public class MessageKafkaRepository : IMessageRepository
{
    private readonly ILogger<MessageKafkaRepository> _logger;
    private readonly KafkaSettings _kafkaSettings;

    private readonly ProducerConfig _producerConfig;
    private readonly SchemaRegistryConfig _schemaRegistryConfig;
    private readonly CachedSchemaRegistryClient _schemaRegistry;

    public MessageKafkaRepository(IOptions<KafkaSettings> kafkaSettings, ILogger<MessageKafkaRepository> logger)
    {
        _logger = logger;
        _kafkaSettings = kafkaSettings.Value;

        _producerConfig = new ProducerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers
        };

        _schemaRegistryConfig = new SchemaRegistryConfig
        {
            // Note: you can specify more than one schema registry url using the
            // schema.registry.url property for redundancy (comma separated list). 
            // The property name is not plural to follow the convention set by
            // the Java implementation.
            Url = _kafkaSettings.SchemaRegistryUrl,
        };

        _schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfig);
    }

    public async Task SendMessageAsync<K, V>(K key, V message, string topicName) where V : IMessage<V>, new()
    {
        using (var producer =
            new ProducerBuilder<K, V>(_producerConfig)
                .SetValueSerializer(new ProtobufSerializer<V>(_schemaRegistry))
                .Build())
        {
            var output = await producer
                .ProduceAsync(topicName, new Message<K, V> { Key = key, Value = message })
                .ContinueWith(task => task.IsFaulted
                    ? $"error producing message: {task.Exception.Message}"
                    : $"produced to: {task.Result.TopicPartitionOffset}");

            _logger.LogInformation(output);
        }
    }
}
