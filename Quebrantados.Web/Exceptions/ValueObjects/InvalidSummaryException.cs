namespace quebrantados.Exceptions.ValueObjects;

public class InvalidSummaryException(string message) : BaseException(message)
{
    public static void ThrowIfInvalid(string value)
    {

        if (string.IsNullOrWhiteSpace(value))
            ThrowIf(true, new InvalidSummaryException("O resumo não pode estar vazio!"));

        if (value.Length is < 20 or > 200)
            ThrowIf(true, new InvalidSummaryException("O resumo deve conter entre 20 e 200 caracteres!"));

    }
}