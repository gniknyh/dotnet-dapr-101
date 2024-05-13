namespace Link.Mydapr.Util.Pubsub;

public interface IEventBus
{
    public Task PublishEvent(IntegrationEvent integrationEvent);
}
