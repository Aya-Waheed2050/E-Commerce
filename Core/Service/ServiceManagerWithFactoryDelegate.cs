﻿namespace Service
{
    internal class ServiceManagerWithFactoryDelegate (
        Func<IProductService> ProductFactory ,
        Func<IBasketService>  BasketFactory,
        Func<IAuthenticationService> AuthenticationFactory,
        Func<IOrderService> OrderFactory,
        Func<IPaymentService> PaymentFactory) : IServiceManager
    {
        public IProductService ProductService => ProductFactory.Invoke();

        public IBasketService BasketService => BasketFactory.Invoke();

        public IAuthenticationService authenticationService => AuthenticationFactory.Invoke();

        public IOrderService OrderService => OrderFactory.Invoke();

        public IPaymentService PaymentService => PaymentFactory.Invoke();
    }
}
