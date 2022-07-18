using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using System.Security.Claims;
using Northstar.Message;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Models;
using HotChocolate.Subscriptions;
using NorthStarGraphQL.GraphQL;
using Microsoft.Extensions.Options;
using CommonLib.Model;

namespace NorthStarGraphQL.Backgrondservices
{
    public class KafkaMessagerService : IHostedService
    {
        private readonly ILogger<KafkaMessagerService> _logger;
        private readonly IIdentityService _identityService;
        private readonly ITopicEventSender _topicEventSender;
        private readonly KafkaSetting _kafkaSettings;

        public KafkaMessagerService(ILogger<KafkaMessagerService> logger, IIdentityService identityService, ITopicEventSender topicEventSender, IOptions<KafkaSetting> kafkaSettings)
        {
            _logger = logger;
            _identityService = identityService;
            _topicEventSender = topicEventSender;
            _kafkaSettings = kafkaSettings.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string bootstrapServers = _kafkaSettings.BootstrapServers;
            string schemaRegistryUrl = _kafkaSettings.SchemaRegistryUrl;
            string topicName = _kafkaSettings.Topics["customermanager"];

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServers
            };

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
                GroupId = _kafkaSettings.GroupId
            };

            CancellationTokenSource cts = new CancellationTokenSource();
            var consumeTask = Task.Run(async () =>
            {
                using (var consumer =
                    new ConsumerBuilder<string, CustomerKafkaMessage>(consumerConfig)
                        .SetValueDeserializer(new ProtobufDeserializer<CustomerKafkaMessage>().AsSyncOverAsync())
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
                                    case CustomerKafkaMessage.MessageTypeOneofCase.None:
                                        Console.WriteLine("None");
                                        break;
                                    case CustomerKafkaMessage.MessageTypeOneofCase.NewUserMessage:
                                        var newUserMessage = consumeResult.Message.Value.NewUserMessage;

                                        _logger.LogInformation($"NewUserMessage was added with id {newUserMessage.Id}");


                                        await _topicEventSender.SendAsync(nameof(NorthStarSubscription.NewUserAdded), new UserCreateItem(
                                            newUserMessage.Id,
                                            newUserMessage.FirstName,
                                            newUserMessage.LastName,
                                            newUserMessage.Email,
                                            newUserMessage.Street,
                                            newUserMessage.Zip,
                                            newUserMessage.City,
                                            0
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
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
