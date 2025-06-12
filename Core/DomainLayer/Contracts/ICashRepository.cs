namespace DomainLayer.Contracts
{
    public interface ICashRepository
    {

        Task<string?> GetAsync(string CashKey);
        Task SetAsync(string CashKey , string CashValue , TimeSpan TimeToLive);

    }
}
