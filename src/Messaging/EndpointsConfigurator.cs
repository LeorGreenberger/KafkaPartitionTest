using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Silverback.Messaging.Configuration;

namespace KafkaPartitionTest.Messaging;

public class EndpointsConfigurator : IEndpointsConfigurator
{
    public void Configure(IEndpointsConfigurationBuilder builder)
    {
        var connectionSettings = builder.ServiceProvider.GetRequiredService<KafkaConnectionSettings>();

        builder.AddKafkaEndpoints(endpoints =>
        {
            endpoints
                .Configure(config =>
                {
                    config.BootstrapServers = connectionSettings.BootstrapServers;
                    config.AllowAutoCreateTopics = false;
                })
                .AddInbound<FooMessage>(
                    endpoint =>
                        endpoint.ConsumeFrom("kafka-partion-test")
                                .Configure(config =>
                                {
                                    config.GroupId = "local_test";
                                    config.AutoOffsetReset = AutoOffsetReset.Earliest;
                                    config.PartitionAssignmentStrategy = PartitionAssignmentStrategy.RoundRobin;
                                })
                                .ProcessAllPartitionsTogether()
                                .OnError(policy => policy.Skip())
                                .SkipNullMessages());
        });
    }
}
