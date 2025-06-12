using Stripe;
using Product = DomainLayer.Models.ProductModule.Product;
using Order = DomainLayer.Models.OrderModule.Order;
namespace Service
{
    internal class PaymentService(IConfiguration _configuration,
        IBasketRepositories _basketRepositories,
        IUnitOfWork _unitOfWork,
        IMapper _mapper)
        : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var basket = await _basketRepositories.GetBasketAsync(basketId) ??
                  throw new BasketNotFoundException(basketId);

            var productRepo = _unitOfWork.GetRepository<Product, int>();

            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }
            ArgumentNullException.ThrowIfNull(basket.deliveryMethodId);
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.deliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(basket.deliveryMethodId.Value);
            basket.shippingPrice = DeliveryMethod.Cost;

            var basketAmount = (long)(basket.Items.Sum(item => item.Price * item.Quantity) + DeliveryMethod.Cost) * 100;

            var paymentService = new PaymentIntentService();
            if (basket.paymentIntentId is null)
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = basketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var PaymentIntent = await paymentService.CreateAsync(options);
                basket.clientSecret = PaymentIntent.ClientSecret;
                basket.paymentIntentId = PaymentIntent.Id;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = basketAmount,
                };
                await paymentService.UpdateAsync(basket.paymentIntentId, options);
            }
            await _basketRepositories.CreateOrUpdateAsync(basket);
            return _mapper.Map<BasketDto>(basket);
        }

        public async Task UpdatePaymentStatus(string request, string stripeHeaders)
        {

            var endPointSecret = _configuration["StripeSettings:EndPointSecret"];
            var stripeEvent = EventUtility.ConstructEvent(request, stripeHeaders, endPointSecret , throwOnApiVersionMismatch:false);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                await UpdatePaymentStatusSucceeded(paymentIntent.Id);
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                await UpdatePaymentStatusFailed(paymentIntent.Id);

            }
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }

        }

        private async Task UpdatePaymentStatusFailed(string PaymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var OrderIntentIdSpec = new OrderWithPaymentIntentIdSpecifications(PaymentIntentId);
            var order = await orderRepo.GetByIdAsync(OrderIntentIdSpec);
            order.Status = OrderStatus.paymentFailed;
            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task UpdatePaymentStatusSucceeded(string PaymentIntentId)
        {
            var orderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var OrderIntentIdSpec = new OrderWithPaymentIntentIdSpecifications(PaymentIntentId);
            var order = await orderRepo.GetByIdAsync(OrderIntentIdSpec);
            order.Status = OrderStatus.paymentReceived;
            orderRepo.Update(order);
            await _unitOfWork.SaveChangesAsync();
        }


    }

}




