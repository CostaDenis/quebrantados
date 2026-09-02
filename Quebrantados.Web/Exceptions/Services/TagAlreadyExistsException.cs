namespace Quebrantados.Web.Exceptions.Services;

public class TagAlreadyExistsException(
    string message = "Já existe uma tag com esse nome ou slug!")
    : BaseException(message);
