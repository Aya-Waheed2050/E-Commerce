namespace Service
{
    public class BasketService(IBasketRepositories _basketRepositories , IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> GetBasketAsync(string key)
        {
            var basket = await _basketRepositories.GetBasketAsync(key);
            if (basket is not null)
                return _mapper.Map<CustomerBasket, BasketDto>(basket);
            else
                throw new BasketNotFoundException(key);
        }

        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket)
        {
           var CustomerBasket  = _mapper.Map<BasketDto , CustomerBasket>(basket);
           var IsCreatedOrUpdated = await _basketRepositories.CreateOrUpdateAsync(CustomerBasket);
            if (IsCreatedOrUpdated is not null)
                return await GetBasketAsync(basket.Id);
            else
                throw new Exception("Can Not Update Or Create Basket Now ,Try Again Later");
        }

        public async Task<bool> DeleteBasketAsync(string key)
         => await _basketRepositories.DeleteBasketAsync(key);
        

        
    }
}
