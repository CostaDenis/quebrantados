namespace Quebrantados.Web.Exceptions.Entities;

public class VisitorWasNotInformedException(string message)
    : BaseException(message)
{

    public static void ThrowIfInvalid(Guid id)
    {
        if (id == Guid.Empty)
            throw new VisitorWasNotInformedException("É necessário informar o Visitante!");
    }

}