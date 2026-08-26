namespace quebrantados.Exceptions.ValueObjects;

public class InvalidBodyException(string message) : BaseException(message)
{
    public static void ThrowIfInvalid(string value)
    {

        if (string.IsNullOrWhiteSpace(value))
            ThrowIf(true, new InvalidBodyException("O texto não pode estar vazio!"));

        if (value.Length is < 100 or > 100000)
            ThrowIf(true, new InvalidBodyException("O texto deve conter entre 100 e 100.000 caracteres!"));

    }
}