using Silverback.Messaging.Broker.Callbacks;

namespace KafkaPartitionTest.Messaging.Topics;

public class TopicsCreationCallbackHandler : IEndpointsConfiguredCallback
{
    private readonly IKafkaTopicsCreator _kafkaTopics;

    public TopicsCreationCallbackHandler(IKafkaTopicsCreator kafkaTopics)
    {
        _kafkaTopics = kafkaTopics;
    }

    public async Task OnEndpointsConfiguredAsync()
    {
        await _kafkaTopics.CreateTopicsAsync();
    }
}