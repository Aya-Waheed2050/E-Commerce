
namespace DomainLayer.Contracts
{
    public interface IBasketRepositories
    {
        Task<CustomerBasket?> GetBasketAsync(string Key);
        Task<CustomerBasket?> CreateOrUpdateAsync(CustomerBasket customerBasket , TimeSpan? timeToLive = null);
        Task<bool> DeleteBasketAsync(string id);

    }
}
