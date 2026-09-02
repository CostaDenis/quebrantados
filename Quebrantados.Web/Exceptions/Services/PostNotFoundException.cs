namespace Quebrantados.Web.Exceptions.Services;

public class PostNotFoundException(string message = "Post não encontrado!")
    : BaseException(message);