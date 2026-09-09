using System.Net.Mail;

namespace Quebrantados.Web.Exceptions.ValueObjects;

public class InvalidEmailAddressException(string message)
    : BaseException(message)
{

    public static void ThrowIfInvalid(string value)
    {

        if (value.Length > 255)
            throw new InvalidEmailAddressException("O Email deve conter até 255 caracteres!");

        if (!MailAddress.TryCreate(value, out _))
            throw new InvalidEmailAddressException("Informe um Email válido!");

    }
}