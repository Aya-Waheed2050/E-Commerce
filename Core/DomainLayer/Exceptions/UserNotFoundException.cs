namespace DomainLayer.Exceptions
{
    sealed public class UserNotFoundException(string email) 
        : NotFoundException($"User With Email {email} Is Not Found")
    {
    }
}
