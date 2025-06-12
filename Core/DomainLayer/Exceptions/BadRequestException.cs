namespace DomainLayer.Exceptions
{
    sealed public class BadRequestException(List<string> errors) : Exception("Validation Failed")
    {
        public List<string> errors { get; } = errors;
    }
}
