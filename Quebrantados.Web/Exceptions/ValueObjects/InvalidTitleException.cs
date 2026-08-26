namespace Quebrantados.Web.Exceptions.ValueObjects;

public class InvalidTitleException(string message)
    : BaseException(message)
{
    public static void ThrowIfInvalid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            ThrowIf(true, new InvalidTitleException("O Título não pode ser nulo!"));

        if (value.Length is < 5 or > 120)
            ThrowIf(true, new InvalidTitleException("O Título deve conter entre 5 e 120 caracteres!"));

    }
}