using Shared.DataTransferObjects.IdentityDtos;

namespace Shared.DataTransferObjects.OrderDtos
{
    public class OrderDto
    {

        public string BasketId { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
        public AddressDto shipToAddress { get; set; } = default!;

    }

}
