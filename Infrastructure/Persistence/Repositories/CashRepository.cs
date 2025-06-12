using DomainLayer.Contracts;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    public class CashRepository(IConnectionMultiplexer _connection) : ICashRepository
    {

        readonly IDatabase _database = _connection.GetDatabase();

        public async Task<string?> GetAsync(string CashKey)
        {
            var CashValue = await _database.StringGetAsync(CashKey);
            return CashValue.IsNullOrEmpty ? null : CashKey.ToString();
        }

        public async Task SetAsync(string CashKey, string CashValue, TimeSpan TimeToLive)
        {
           await _database.StringSetAsync(CashKey , CashValue , TimeToLive);
        }
    }
}
