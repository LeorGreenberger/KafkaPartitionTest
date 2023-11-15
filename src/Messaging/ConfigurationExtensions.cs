using KafkaPartitionTest.Messaging.Topics;
using Microsoft.Extensions.DependencyInjection;
using Silverback.Messaging.Configuration;

namespace KafkaPartitionTest.Messaging;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSilverback()
                .WithConnectionToMessageBroker(options => options.AddKafka())
                .AddKafkaConnectionSettings(options =>
                {
                    options.BootstrapServers = "localhost:9092";
                    options.KafkaTopicCreation.CreateKafkaTopics = true;
                    options.KafkaTopicCreation.Partitions = 1;
                })
                .CreateKafkaTopics()
                .AddEndpointsConfigurator<EndpointsConfigurator>()
                .AddDelegateSubscriber<FooMessage>(m => Console.WriteLine($"Message received: {m.Value}"));

        return services;
    }

    private static ISilverbackBuilder CreateKafkaTopics(this ISilverbackBuilder builder)
    {
        builder.Services.AddSingleton<IKafkaTopicsProvider, ConsumedKafkaTopicsProvider>();
        builder.Services.AddSingleton<IKafkaTopicsProvider, ProducedKafkaTopicsProvider>();
        builder.Services.AddSingleton<IKafkaTopicsCreator, KafkaTopicsCreator>();
        builder.AddSingletonBrokerCallbackHandler<TopicsCreationCallbackHandler>();

        return builder;
    }

    private static ISilverbackBuilder AddKafkaConnectionSettings(this ISilverbackBuilder builder, Action<KafkaConnectionSettings> optionsAction)
    {
        ArgumentNullException.ThrowIfNull(optionsAction, nameof(optionsAction));

        var options = new KafkaConnectionSettings();

        optionsAction(options);

        ArgumentException.ThrowIfNullOrEmpty(options.BootstrapServers, nameof(options.BootstrapServers));

        builder.Services.AddSingleton(options);

        return builder;
    }
}
