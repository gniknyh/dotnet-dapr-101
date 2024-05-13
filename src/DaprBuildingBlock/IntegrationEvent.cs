namespace Link.Mydapr.Util.Pubsub
{
    public record IntegrationEvent
    {
        public Guid Id {get;}

        public DateTime CreatedTime {get;}

        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedTime = DateTime.UtcNow;
        }
    }
}