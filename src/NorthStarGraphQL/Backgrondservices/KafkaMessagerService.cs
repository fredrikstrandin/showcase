using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using System.Security.Claims;
using Northstar.Message;

namespace NorthStarGraphQL.Backgrondservices
{
    public class KafkaMessagerService : IHostedService
    {

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            string bootstrapServers = "localhost:9092";
            string schemaRegistryUrl = "localhost:8081";
            string topicName = "customermanager";

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
                GroupId = "northstar-graphql-group"
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
                                        Console.WriteLine($"Firstname: {consumeResult.Message.Value.NewUserMessage.FirstName}");
                                        Console.WriteLine($"LastName: {consumeResult.Message.Value.NewUserMessage.LastName}");
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
