namespace Quebrantados.Web.Exceptions.ValueObjects;

public class InvalidCommentContentException(string message)
    : BaseException(message)
{

    public static void ThrowIfInvalid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidCommentContentException("Informe o Comentário!");

        if (value.Length is < 3 or > 1000)
            throw new InvalidCommentContentException("O Comentário deve ter entre 3 e 1000 caracteres!");

    }
}