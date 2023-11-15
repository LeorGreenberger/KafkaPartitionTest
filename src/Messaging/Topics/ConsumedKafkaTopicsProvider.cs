using Silverback.Messaging.Broker;

namespace KafkaPartitionTest.Messaging.Topics;

internal class ConsumedKafkaTopicsProvider : IKafkaTopicsProvider
{
    private readonly IBrokerCollection _brokers;

    public ConsumedKafkaTopicsProvider(IBrokerCollection brokers)
    {
        _brokers = brokers;
    }

    public IList<string> GetKafkaTopics()
    {
        var kafkaBroker = _brokers.OfType<KafkaBroker>().FirstOrDefault();

        if (kafkaBroker == null)
        {
            return Array.Empty<string>();
        }

        return kafkaBroker.Consumers.OfType<KafkaConsumer>().Select(c => c.Endpoint.Name).ToList();
    }
}