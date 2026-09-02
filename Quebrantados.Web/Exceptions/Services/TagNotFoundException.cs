namespace Quebrantados.Web.Exceptions.Services;

public class TagNotFoundException(string message = "Tag não encontrada!")
    : BaseException(message);