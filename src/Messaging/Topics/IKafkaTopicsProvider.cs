namespace KafkaPartitionTest.Messaging.Topics;

public interface IKafkaTopicsProvider
{
    IList<string> GetKafkaTopics();
}