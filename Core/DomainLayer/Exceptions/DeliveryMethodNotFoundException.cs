namespace DomainLayer.Exceptions
{
    sealed public class DeliveryMethodNotFoundException(int id) : NotFoundException($"No Delivery Method with id : {id}")
    {
    }
}
