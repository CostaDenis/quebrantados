namespace Quebrantados.Web.Exceptions.Services;

public class CommentNotFoundException(string message = "Comentário não encontrado!")
    : BaseException(message);