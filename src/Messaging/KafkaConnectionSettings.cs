using KafkaPartitionTest.Messaging.Topics;

namespace KafkaPartitionTest.Messaging;

public class KafkaConnectionSettings
{
    public string BootstrapServers { get; set; } = default!;
    public KafkaTopicCreation KafkaTopicCreation { get; set; } = new();
}