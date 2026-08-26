namespace Quebrantados.Web.Exceptions.ValueObjects;

public class InvalidCategoryNameException(string message) : BaseException(message)
{
    public static void ThrowIfInvalid(string value)
    {

        if (string.IsNullOrWhiteSpace(value))
            ThrowIf(true, new InvalidCategoryNameException("O nome da etiqueta é obrigatório!"));

        if (value.Length is < 2 or > 60)
            ThrowIf(true, new InvalidCategoryNameException("O nome da etiqueta deve conter entre 2 e 60 caracteres!"));
    }
}