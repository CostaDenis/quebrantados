namespace Quebrantados.Web.Exceptions.Services;

public class PostAlreadyExistsException(string message = "Já existe um Post com esse título ou slug!")
    : BaseException(message);
