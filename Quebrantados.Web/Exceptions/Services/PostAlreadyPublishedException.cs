namespace Quebrantados.Web.Exceptions.Services;

public class PostAlreadyPublishedException(string message = "O Post já está publicado!")
    : BaseException(message);
