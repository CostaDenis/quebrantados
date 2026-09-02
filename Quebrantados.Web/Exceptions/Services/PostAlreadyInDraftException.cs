namespace Quebrantados.Web.Exceptions.Services;

public class PostAlreadyInDraftException(string message = "O Post já está rascunho!")
    : BaseException(message);
