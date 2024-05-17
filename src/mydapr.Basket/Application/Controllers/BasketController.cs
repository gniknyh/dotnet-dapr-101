using Link.Mydapr.Service.Basket.Events;
using Link.Mydapr.Service.Basket.Infrastucture;
using Link.Mydapr.Service.Basket.Model;
using Link.Mydapr.Util.Pubsub;
using Microsoft.AspNetCore.Mvc;

namespace Link.Mydapr.Service.Basket.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController  : ControllerBase
    {

        public BasketController(IBasketRepository basketRepository,
        IEventBus eventBus
        )
        {
            _basketRepository = basketRepository;
            _eventBus = eventBus;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket([FromQuery]int customerId)
        {
            var basket = await _basketRepository.GetAsync(customerId);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket( CustomerBasket customerBasket)
        {
            var basket = await _basketRepository.UpdateAsync(customerBasket);
            return Ok(basket);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult> Checkout([FromBody]int customerId)
        {
            var basket = await _basketRepository.GetAsync(customerId);
            var checkEvent = new CheckoutIntegrationEvent(basket);
            await _eventBus.PublishEvent(checkEvent);
            return Ok();
        }

        private IBasketRepository _basketRepository;
        
        private IEventBus _eventBus;

    }
}
