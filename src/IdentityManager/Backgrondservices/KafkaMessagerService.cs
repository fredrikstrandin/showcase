using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using IdentityManager.Interface;
using Showcase.Message;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityManager.Backgrondservices
{
    public class KafkaMessagerService : IHostedService
    {
        private readonly IUserStoreService _userStoreService;


        public KafkaMessagerService(IUserStoreService userStoreService)
        {
            _userStoreService = userStoreService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string bootstrapServers = "localhost:9092";
            string schemaRegistryUrl = "localhost:8081";
            string topicName = "identity";

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
                GroupId = "protobuf-example-consumer-group"
            };

            CancellationTokenSource cts = new CancellationTokenSource();
            var consumeTask = Task.Run(async () =>
            {
                using (var consumer =
                    new ConsumerBuilder<string, IdentityMessage>(consumerConfig)
                        .SetValueDeserializer(new ProtobufDeserializer<IdentityMessage>().AsSyncOverAsync())
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

                                Console.WriteLine($"Id: {consumeResult.Message.Value.Id}");

                                switch (consumeResult.Message.Value.MessageTypeCase)
                                {
                                    case IdentityMessage.MessageTypeOneofCase.None:
                                        Console.WriteLine("None");
                                        break;
                                    case IdentityMessage.MessageTypeOneofCase.Person:
                                        Console.WriteLine($"Firstname: {consumeResult.Message.Value.Person.Firstname}");
                                        break;
                                    case IdentityMessage.MessageTypeOneofCase.Login:
                                        Console.WriteLine($"Nickname: {consumeResult.Message.Value.Login.Nickname}");

                                        List<Claim> claims = new List<Claim>();

                                        foreach (var item in consumeResult.Message.Value.Login.Claims)
                                        {
                                            claims.Add(new Claim(item.Type, item.Value));
                                        }

                                        string id = await _userStoreService.AddUserAsync(consumeResult.Message.Value.Login.Nickname, 
                                            consumeResult.Message.Value.Login.Password,
                                            consumeResult.Message.Value.Login.Salt.ToArray(),
                                            claims);

                                        Console.WriteLine($"Id: {id}");

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
