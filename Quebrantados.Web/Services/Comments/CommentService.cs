using Quebrantados.Web.DTOs.Comments;
using Quebrantados.Web.Entities;
using Quebrantados.Web.Enums;
using Quebrantados.Web.Exceptions.Services;
using Quebrantados.Web.Repositories.Comments;
using Quebrantados.Web.Repositories.Posts;
using Quebrantados.Web.ValueObjects;

namespace Quebrantados.Web.Services.Comments;

public class CommentService(ICommentRepository commentRepository,
    IPostRepository postRepository)
    : ICommentService
{
    public async Task<List<CommentAdminOutput>> GetByStatusAsync(ECommentStatus status, CancellationToken cancellationToken)
    {
        var comments = await commentRepository.GetByStatusAsync(status, cancellationToken);

        return [.. comments.Select(comment => new CommentAdminOutput(
            comment.Id,
            comment.PostId,
            comment.AuthorName.Value,
            comment.AuthorEmail?.Value,
            comment.Content.Value,
            comment.Status,
            comment.CreatedAt))];
    }

    public async Task<List<CommentOutput>> GetApprovedByPostIdAsync(Guid postId, CancellationToken cancellationToken)
    {
        var comments = await commentRepository.GetApprovedByPostIdAsync(postId, cancellationToken);

        return [.. comments.Select(comment => new CommentOutput(
            comment.Id,
            comment.AuthorName.Value,
            comment.Content.Value,
            comment.CreatedAt
        ))];
    }

    public async Task<List<CommentAdminOutput>> GetAllAsync(CancellationToken cancellationToken)
    {
        var comments = await commentRepository.GetAllAsync(cancellationToken);

        return [.. comments.Select(comment => new CommentAdminOutput(
            comment.Id,
            comment.PostId,
            comment.AuthorName.Value,
            comment.AuthorEmail?.Value,
            comment.Content.Value,
            comment.Status,
            comment.CreatedAt))];
    }

    public async Task CreateAsync(CommentInput input, CancellationToken cancellationToken)
    {
        var postExists = await postRepository.ExistsPublishedAsync(input.PostId, cancellationToken);

        if (!postExists)
            throw new PostNotFoundException();

        var authorEmail = string.IsNullOrWhiteSpace(input.AuthorEmail)
            ? null
            : new EmailAddress(input.AuthorEmail);

        var comment = new Comment(
            input.PostId,
            new CommentAuthorName(input.AuthorName),
            authorEmail,
            new CommentContent(input.Content));

        await commentRepository.CreateAsync(comment, cancellationToken);
    }

    public async Task ApproveAsync(Guid id, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new CommentNotFoundException();

        comment.Approve();
        await commentRepository.UpdateAsync(comment, cancellationToken);
    }

    public async Task RejectAsync(Guid id, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new CommentNotFoundException();

        comment.Reject();
        await commentRepository.UpdateAsync(comment, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var comment = await commentRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new CommentNotFoundException();

        await commentRepository.DeleteAsync(comment, cancellationToken);
    }

}
