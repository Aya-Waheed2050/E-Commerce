namespace DomainLayer.Exceptions
{
    sealed public class BasketNotFoundException(string id) : NotFoundException($"Basket With Id : {id} Is Not Found")
    {
    }
}
