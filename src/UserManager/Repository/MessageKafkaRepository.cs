using CommonLib.Settings;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Northstar.Message;
using System;
using System.Threading.Tasks;
using UserManager.Interfaces;

namespace UserManager.Repository;

public class MessageKafkaRepository : IMessageRepository
{
    private readonly ILogger<MessageKafkaRepository> _logger;
    private readonly KafkaSetting _kafkaSettings;

    private readonly ProducerConfig _producerConfig;
    private readonly SchemaRegistryConfig _schemaRegistryConfig;
    private readonly CachedSchemaRegistryClient _schemaRegistry;
    private readonly IProducer<string, UserKafkaMessage> _producer;

    public MessageKafkaRepository(IOptions<KafkaSetting> kafkaSettings, ILogger<MessageKafkaRepository> logger)
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
        _producer =
                new ProducerBuilder<string, UserKafkaMessage>(_producerConfig)
                    .SetValueSerializer(new ProtobufSerializer<UserKafkaMessage>(_schemaRegistry))
                    .Build();


        }

    public async Task SendMessageAsync(string key, UserKafkaMessage message, string topicName) 
    {
        try
        {
            var output = await _producer
                .ProduceAsync(topicName, new Message<string, UserKafkaMessage> { Key = key, Value = message })
                .ContinueWith(task => task.IsFaulted
                    ? $"error producing message: {task.Exception.Message}"
                    : $"produced to: {task.Result.TopicPartitionOffset}");

            _logger.LogInformation(output);

        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }
}
