
using Dapr.Client;
using Microsoft.Extensions.Logging;

namespace Link.Mydapr.Util.Pubsub;

public class DaprEventBus : IEventBus
{
    public DaprEventBus(DaprClient daprClient,
    ILogger<DaprEventBus> logger
    )
    {
        _daprClient = daprClient;
        _logger = logger;
    }

 
    public async Task PublishEvent( IntegrationEvent integrationEvent)
    {
        var topic = integrationEvent.GetType().Name;

        _logger.LogInformation(
            "Publishing event {@Event} to {PubsubName}.{TopicName}",
            integrationEvent,
            DAPR_PUBSUB_NAME,
            topic);

        // We need to make sure that we pass the concrete type to PublishEventAsync,
        // which can be accomplished by casting the event to dynamic. This ensures
        // that all event fields are properly serialized.
        await _daprClient.PublishEventAsync(DAPR_PUBSUB_NAME, topic, (object)integrationEvent);
    }

    private DaprClient _daprClient;

    private ILogger _logger;

    private const string DAPR_PUBSUB_NAME = "mydapr-pubsub";
}