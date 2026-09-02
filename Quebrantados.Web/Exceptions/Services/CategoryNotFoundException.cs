namespace Quebrantados.Web.Exceptions.Services;

public class CategoryNotFoundException(string message = "Categoria não encontrada!")
    : BaseException(message);