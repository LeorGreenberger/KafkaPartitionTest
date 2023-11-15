namespace KafkaPartitionTest.Messaging.Topics;

public class KafkaTopicCreation
{
    public bool CreateKafkaTopics { get; set; }
    public int Partitions { get; set; } = -1;
    public short ReplicationFactor { get; set; } = -1;
}