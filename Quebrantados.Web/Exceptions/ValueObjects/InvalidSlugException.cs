using System.Text.RegularExpressions;

namespace Quebrantados.Web.Exceptions.ValueObjects;

public class InvalidSlugException(string message) : BaseException(message)
{
    public static void ThrowIfInvalid(string value, string normalized)
    {

        if (string.IsNullOrWhiteSpace(value))
            ThrowIf(true, new InvalidSlugException("O slug é obrigatório!"));

        if (value.Length is < 3 or > 150)
            ThrowIf(true, new InvalidSlugException("O slug deve conter entre 3 e 150 caracteres!"));

        if (!Regex.IsMatch(normalized, @"^[a-z0-9]+(?:-[a-z0-9]+)*$"))
        {
            throw new InvalidSlugException(
                "O slug deve conter apenas letras minúsculas, números e hífens.");
        }

    }
}