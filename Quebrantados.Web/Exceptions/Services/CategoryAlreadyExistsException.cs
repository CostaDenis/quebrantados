namespace Quebrantados.Web.Exceptions.Services;

public class CategoryAlreadyExistsException(string message = "Já existe uma Categoria com esse nome ou slug!")
    : BaseException(message);
