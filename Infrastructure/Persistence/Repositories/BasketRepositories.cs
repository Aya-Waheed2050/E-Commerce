using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using DomainLayer.Contracts;
using DomainLayer.Models.BasketModule;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    public class BasketRepositories(IConnectionMultiplexer connection) : IBasketRepositories
    {
        private readonly IDatabase _database = connection.GetDatabase();


        public async Task<CustomerBasket?> GetBasketAsync(string Key)
        {
            var Basket = await _database.StringGetAsync(Key);
            if (Basket.IsNullOrEmpty)
                return null;
            else
                return JsonSerializer.Deserialize<CustomerBasket>(Basket!);
            
        }

        public async Task<CustomerBasket?> CreateOrUpdateAsync(CustomerBasket customerBasket, TimeSpan? timeToLive = null)
        {
            var jsonsBasket = JsonSerializer.Serialize(customerBasket);
            var IsCreatedOrUpdated =  await _database.StringSetAsync(customerBasket.Id, jsonsBasket , timeToLive?? TimeSpan.FromDays(30));

            if (IsCreatedOrUpdated)
                return await GetBasketAsync(customerBasket.Id);
            else
                return null;
            
        }

        public async Task<bool> DeleteBasketAsync(string id)
         => await _database.KeyDeleteAsync(id);
        

       
    }
}
