namespace SalesWebMvc.Services.Exceptions
{
    public class DbConcurrencyException(string message) : ApplicationException(message)
    {
    }
}
