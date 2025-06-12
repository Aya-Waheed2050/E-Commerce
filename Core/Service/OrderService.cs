
using Service.Specifications.OrderModuleSpecifications;

namespace Service
{
    public class OrderService(IMapper _mapper , IBasketRepositories _basketRepositories , IUnitOfWork _unitOfWork) 
        : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrderAsync(OrderDto orderDto, string email)
        {
            // Map Address To Order Address
            var OrderAddress = _mapper.Map<AddressDto, OrderAddress>(orderDto.shipToAddress);

            // Get Basket
            var Basket = await _basketRepositories.GetBasketAsync(orderDto.BasketId) 
                      ?? throw new BasketNotFoundException(orderDto.BasketId);

            ArgumentException.ThrowIfNullOrEmpty(Basket.paymentIntentId);
            var OrderRepo = _unitOfWork.GetRepository<Order, Guid>();
            var OrderSpec = new OrderWithPaymentIntentIdSpecifications(Basket.paymentIntentId);
            var ExistingOrder = await OrderRepo.GetByIdAsync(OrderSpec);
            if (ExistingOrder is not null)
                OrderRepo.Remove(ExistingOrder);

            // Create OrderItem
            List<OrderItem> OrderItems = [];

            var ProductRepo = _unitOfWork.GetRepository<Product , int>();

            foreach(var item in Basket.Items)
            {
                var product = await ProductRepo.GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);

                OrderItems.Add(CreateOrderItem(item, product));

            }

            // Get Delivery Method
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                                 .GetByIdAsync(orderDto.DeliveryMethodId)
                ?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);

            // Calculate SubTotal

            var subTotal = OrderItems.Sum(I => I.Quantity * I.Price);

            var order = new Order
            (
                email, OrderAddress, DeliveryMethod, OrderItems, subTotal , Basket.paymentIntentId
            );

            await OrderRepo.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Order , OrderToReturnDto>(order);
        }

        private static OrderItem CreateOrderItem(BasketItem item, Product product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrdered()
                {
                    ProductId = product.Id,
                    PictureUrl = product.PictureUrl,
                    ProductName = product.Name
                },
                Price = product.Price,
                Quantity = item.Quantity
            };
        }

      
        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var DeliveryMethods = await _unitOfWork.GetRepository<DeliveryMethod , int>().GetAllAsync();
            return _mapper.Map<IEnumerable<DeliveryMethod> , IEnumerable<DeliveryMethodDto>>(DeliveryMethods);
        }

        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string email)
        {
            var specification = new OrderSpecifications(email);
            var orders = await _unitOfWork.GetRepository<Order , Guid>().GetAllAsync(specification);
            return _mapper.Map< IEnumerable<Order> ,IEnumerable<OrderToReturnDto>>(orders);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid id)
        {
            var specification = new OrderSpecifications(id);
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(specification);
            return _mapper.Map<Order , OrderToReturnDto>(order);
        }
    }

}
