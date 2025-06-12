namespace DomainLayer.Exceptions
{
    sealed public class ProductNotFoundException(int id) : NotFoundException($"Product With Id : {id} Is Not Found")
    {
    }
}
