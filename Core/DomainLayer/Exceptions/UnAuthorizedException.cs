namespace DomainLayer.Exceptions
{
    sealed public class UnAuthorizedException(string message = "invalid Email Or Password") : Exception(message)
    {
    }
}
