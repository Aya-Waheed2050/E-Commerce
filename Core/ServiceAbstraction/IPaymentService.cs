namespace ServiceAbstraction
{
    public interface IPaymentService
    {
        Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId);

        Task UpdatePaymentStatus(string request, string stripeHeaders);

    }

}
