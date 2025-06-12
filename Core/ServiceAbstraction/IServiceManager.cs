namespace ServiceAbstraction
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
        IBasketService BasketService { get; }
        IAuthenticationService authenticationService { get; }
        IOrderService OrderService { get; }   
        IPaymentService PaymentService { get; }

    }
}
