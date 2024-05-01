using Link.Mydapr.Service.Basket.Infrastucture;
using Link.Mydapr.Service.Basket.Model;
using Microsoft.AspNetCore.Mvc;

namespace Link.Mydapr.Service.Basket.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController  : ControllerBase
    {

        private IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket([FromQuery]int customerId)
        {
            var basket = await _basketRepository.GetAsync(customerId);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket([FromBody] CustomerBasket customerBasket)
        {
            var basket = await _basketRepository.UpdateAsync(customerBasket);
            return Ok(basket);
        }
    }
}
