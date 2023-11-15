using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Logging;
using Silverback.Messaging.Broker.Kafka;

namespace KafkaPartitionTest.Messaging.Topics;

internal class KafkaTopicsCreator : IKafkaTopicsCreator
{
    private readonly IEnumerable<IKafkaTopicsProvider> _topicsProviders;
    private readonly IConfluentAdminClientBuilder _adminClientBuilder;
    private readonly KafkaConnectionSettings _config;
    private readonly ILogger<KafkaTopicsCreator> _logger;

    public KafkaTopicsCreator(IEnumerable<IKafkaTopicsProvider> topicsProviders,
                              IConfluentAdminClientBuilder adminClientBuilder,
                              KafkaConnectionSettings config,
                              ILogger<KafkaTopicsCreator> logger)
    {
        _topicsProviders = topicsProviders;
        _adminClientBuilder = adminClientBuilder;
        _config = config;
        _logger = logger;
    }

    public async Task CreateTopicsAsync()
    {
        if (!_config.KafkaTopicCreation.CreateKafkaTopics)
        {
            return;
        }

        var topics = _topicsProviders
                     .SelectMany(p => p.GetKafkaTopics())
                     .Distinct(StringComparer.InvariantCulture)
                     .ToList();

        if (!topics.Any())
        {
            return;
        }

        var clientConfig = new ClientConfig { BootstrapServers = _config.BootstrapServers };
        using var adminClient = _adminClientBuilder.Build(clientConfig);

        _logger.LogInformation("Executing topic creation for topics: {Topics}", string.Join(',', topics));

        try
        {
            await adminClient.CreateTopicsAsync(topics.Select(t => new TopicSpecification
            {
                Name = t,
                NumPartitions = _config.KafkaTopicCreation.Partitions,
                ReplicationFactor = _config.KafkaTopicCreation.ReplicationFactor
            }));

            _logger.LogInformation("Executed topic creation.");
        }
        catch (CreateTopicsException ex)
        {
            if (ex.Results.All(r =>
                                   r.Error.IsError && r.Error.Code == ErrorCode.TopicAlreadyExists || !r.Error.IsError))
            {
                _logger.LogInformation("Topics already exists.");
                return;
            }

            throw;
        }
    }
}