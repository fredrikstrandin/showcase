using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using CustomerManager.Interfaces;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CustomerManager.Repository;

public class MessageKafkaRepository : IMessageRepository
{
    private readonly ILogger<MessageKafkaRepository> _logger;

    private readonly string _bootstrapServers = "localhost:9092";
    private readonly string _schemaRegistryUrl = "localhost:8081";
    private readonly ProducerConfig _producerConfig;
    private readonly SchemaRegistryConfig _schemaRegistryConfig;

    public MessageKafkaRepository(ILogger<MessageKafkaRepository> logger)
    {
        _logger = logger;

        _producerConfig = new ProducerConfig
        {
            BootstrapServers = _bootstrapServers
        };

        _schemaRegistryConfig = new SchemaRegistryConfig
        {
            // Note: you can specify more than one schema registry url using the
            // schema.registry.url property for redundancy (comma separated list). 
            // The property name is not plural to follow the convention set by
            // the Java implementation.
            Url = _schemaRegistryUrl,
        };

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = _bootstrapServers,
            GroupId = "protobuf-example-consumer-group"
        };
    }

    public async Task SendMessageAsync<K, V>(K key, V message, string topicName) where V : IMessage<V>, new()
    {
        using (var schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfig))
        using (var producer =
            new ProducerBuilder<K, V>(_producerConfig)
                .SetValueSerializer(new ProtobufSerializer<V>(schemaRegistry))
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
