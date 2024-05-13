using Link.Mydapr.Service.Basket.Model;
using Link.Mydapr.Util.Pubsub;

namespace Link.Mydapr.Service.Basket.Events
{
    public record CheckoutIntegrationEvent(CustomerBasket basket) : IntegrationEvent;
}