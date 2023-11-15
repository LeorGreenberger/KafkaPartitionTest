using Silverback.Messaging;
using Silverback.Messaging.Outbound.Routing;

namespace KafkaPartitionTest.Messaging.Topics;

internal class ProducedKafkaTopicsProvider : IKafkaTopicsProvider
{
    private readonly IServiceProvider _serviceProvider;

    private readonly IOutboundRoutingConfiguration _routingConfiguration;

    public ProducedKafkaTopicsProvider(IServiceProvider serviceProvider, IOutboundRoutingConfiguration routingConfiguration)
    {
        _serviceProvider = serviceProvider;
        _routingConfiguration = routingConfiguration;
    }

    public IList<string> GetKafkaTopics()
    {
        return _routingConfiguration.Routes
                                    .SelectMany(r => r.GetOutboundRouter(_serviceProvider).Endpoints)
                                    .OfType<KafkaProducerEndpoint>()
                                    .Select(e => e.Name)
                                    .ToList();
    }
}