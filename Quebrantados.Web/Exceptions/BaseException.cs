namespace Quebrantados.Web.Exceptions;

public class BaseException(string message) : Exception(message)
{
    public static void ThrowIf(bool condition, Exception exception)
    {
        if (condition)
            throw exception;
    }
}