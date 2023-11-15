namespace KafkaPartitionTest.Messaging.Topics;

public interface IKafkaTopicsCreator
{
    Task CreateTopicsAsync();
}