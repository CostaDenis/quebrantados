namespace quebrantados.Exceptions.ValueObjects;

public class InvalidTagNameException(string message) : BaseException(message)
{
    public static void ThrowIfInvalid(string value)
    {

        if (string.IsNullOrWhiteSpace(value))
            ThrowIf(true, new InvalidTagNameException("O nome da etiqueta é obrigatório!"));

        if (value.Length is < 2 or > 50)
            ThrowIf(true, new InvalidTagNameException("O nome da etiqueta deve conter entre 2 e 50 caracteres!"));
    }
}