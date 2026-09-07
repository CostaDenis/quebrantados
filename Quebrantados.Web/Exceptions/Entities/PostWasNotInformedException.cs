namespace Quebrantados.Web.Exceptions.Entities;

public class PostWasNotInformedException(string message)
    : BaseException(message)
{

    public static void ThrowIfInvalid(Guid id)
    {
        if (id == Guid.Empty)
            throw new PostWasNotInformedException("É necessário informar o Post!");
    }

}