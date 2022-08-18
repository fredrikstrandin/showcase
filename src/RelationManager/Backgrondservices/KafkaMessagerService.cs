using CommonLib.Settings;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Options;
using Northstar.Message;
using RelationManager.Interface;
using RelationManager.Models;

namespace RelationManager.Backgrondservices;

public class KafkaMessagerService : IHostedService
{
    private readonly ILogger<KafkaMessagerService> _logger;
    private readonly IUserService _userService;
    private readonly KafkaSetting _kafkaSettings;

    public KafkaMessagerService(ILogger<KafkaMessagerService> logger, IOptions<KafkaSetting> kafkaSettings, IUserService userService)
    {
        _logger = logger;
        _kafkaSettings = kafkaSettings.Value;
        _userService = userService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        string bootstrapServers = _kafkaSettings.BootstrapServers;
        string schemaRegistryUrl = _kafkaSettings.SchemaRegistryUrl;
        string topicName = _kafkaSettings.Topics["usermanager"];

        var schemaRegistryConfig = new SchemaRegistryConfig
        {
            // Note: you can specify more than one schema registry url using the
            // schema.registry.url property for redundancy (comma separated list). 
            // The property name is not plural to follow the convention set by
            // the Java implementation.
            Url = schemaRegistryUrl,
        };

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            AutoOffsetReset = AutoOffsetReset.Latest,
            GroupId = _kafkaSettings.GroupId
        };

        var consumeTask = Task.Run(async () =>
        {
            using (var consumer =
                new ConsumerBuilder<string, UserKafkaMessage>(consumerConfig)
                    .SetValueDeserializer(new ProtobufDeserializer<UserKafkaMessage>().AsSyncOverAsync())
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build())
            {
                consumer.Subscribe(topicName);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(cancellationToken);

                            switch (consumeResult.Message.Value.MessageTypeCase)
                            {
                                case UserKafkaMessage.MessageTypeOneofCase.None:
                                    Console.WriteLine("None");
                                    break;
                                case UserKafkaMessage.MessageTypeOneofCase.NewUserMessage:
                                    var newUserMessage = consumeResult.Message.Value.NewUserMessage;

                                    _logger.LogInformation($"NewUserMessage was added with id {newUserMessage.Id}");


                                    await _userService.AddAsync(new UserAddItem(
                                        newUserMessage.Id,
                                        newUserMessage.FirstName,
                                        newUserMessage.LastName
                                    ));

                                    break;
                                default:
                                    Console.WriteLine("Error");
                                    break;
                            }
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Consume error: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }

        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
