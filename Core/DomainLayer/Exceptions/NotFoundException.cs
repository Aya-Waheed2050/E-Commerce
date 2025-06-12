namespace DomainLayer.Exceptions
{
    abstract public class NotFoundException(string Message) : Exception(Message)
    {
    }
}
