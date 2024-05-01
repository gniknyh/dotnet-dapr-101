using Link.Mydapr.Service.Basket.Model;

namespace Link.Mydapr.Service.Basket.Infrastucture
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket> GetAsync(int customerId);

        public Task<CustomerBasket> UpdateAsync(CustomerBasket customerBasket);

        public Task RemoveAsync(int customerId);

    }
}