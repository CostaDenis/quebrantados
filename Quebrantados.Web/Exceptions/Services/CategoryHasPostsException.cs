namespace Quebrantados.Web.Exceptions.Services;

public class CategoryHasPostsException(string message = "Categoria com Posts não pode ser excluída!")
    : BaseException(message);