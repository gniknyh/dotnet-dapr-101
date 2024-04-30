using Dapr.Client;
using Link.Mydapr.Service.Basket.Model;

namespace Link.Mydapr.Service.Basket.Infrastucture
{
    public class BasketRepository : IBasketRepository
    {

        public BasketRepository(ILogger<BasketRepository> logger, DaprClient daprClient )
        {
            _logger = logger;
            _daprClient = daprClient;

        }

        public async Task<CustomerBasket> GetAsync(int customerId)
        {
            _logger.LogInformation($"receive customerId {customerId}");
            var basket = await _daprClient.GetStateAsync<CustomerBasket>(_storeName, customerId.ToString());
            return basket;
        }

        public async Task<CustomerBasket> UpdateAsync(CustomerBasket customerBasket)
        {
            var state = await _daprClient.GetStateEntryAsync<CustomerBasket>(_storeName, customerBasket.CustomerId.ToString());
            if (state == null)
            {
                await _daprClient.SaveStateAsync(_storeName, customerBasket.CustomerId.ToString(), customerBasket);
            }
            else
            {
                state.Value = customerBasket;
                await state.SaveAsync();

            }
            _logger.LogInformation("save basket");
            return await GetAsync(customerBasket.CustomerId);
        }

        private readonly ILogger _logger;
        private readonly DaprClient _daprClient;

        private const string _storeName = "mydapr-statestore";
        
    }
}