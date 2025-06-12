namespace DomainLayer.Exceptions
{
    sealed public class AddressNotFoundException(string userName) : NotFoundException($"user {userName} has no address")
    {
    }
}
