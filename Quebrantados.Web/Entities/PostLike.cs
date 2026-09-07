using Quebrantados.Web.Exceptions.Entities;

namespace Quebrantados.Web.Entities;

public class PostLike : Entity
{
    private PostLike()
    { }

    public PostLike(Guid postId, Guid visitorId)
    {
        PostWasNotInformedException.ThrowIfInvalid(postId);
        VisitorWasNotInformedException.ThrowIfInvalid(visitorId);

        PostId = postId;
        VisitorId = visitorId;
    }

    public Guid PostId { get; private set; }
    public Post Post { get; private set; } = null!;
    public Guid VisitorId { get; private set; }
}