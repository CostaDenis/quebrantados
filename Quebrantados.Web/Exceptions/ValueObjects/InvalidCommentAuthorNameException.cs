namespace Quebrantados.Web.Exceptions.ValueObjects;

public class InvalidCommentAuthorNameException(string message)
    : BaseException(message)
{

    public static void ThrowIfInvalid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidCommentAuthorNameException("Deve informar o nome do Autor do comentário!");

        if (value.Length is > 80 or < 2)
            throw new InvalidCommentAuthorNameException("O nome do Autor do comentário deve ter entre 2 e 80 caracteres!");

    }
}