using System.Text.Json;

namespace Service
{
    internal class CashService(ICashRepository _cashRepository) : ICashService
    {
        public Task<string?> GetAsync(string CashKey)
        {
            return _cashRepository.GetAsync(CashKey);
        }

        public async Task SetAsync(string CashKey, object CashValue, TimeSpan TimeToLive)
        {
            var value = JsonSerializer.Serialize(CashValue);
            await _cashRepository.SetAsync(CashKey , value , TimeToLive);
        }

    }

}
