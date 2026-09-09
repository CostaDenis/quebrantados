using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Comments;
using Quebrantados.Web.Enums;
using Quebrantados.Web.Exceptions;
using Quebrantados.Web.Services.Comments;

namespace Quebrantados.Web.Components.Pages.Dashboard.Comments;

public partial class CommentsPage : ComponentBase
{
    [Inject] public ICommentService CommentService { get; set; } = null!;
    [Inject] public ILogger<CommentsPage> Logger { get; set; } = null!;

    protected List<CommentAdminOutput>? CommentItems { get; set; }
    protected ECommentStatus? StatusFilter { get; set; } = ECommentStatus.Pending;
    protected string SearchTerm { get; set; } = string.Empty;
    protected string? NotificationMessage { get; set; }
    protected bool NotificationIsError { get; set; }
    protected bool IsDeleting { get; set; }

    protected Guid? _processingCommentId;
    protected CommentAdminOutput? _commentPendingDeletion;

    protected int TotalCount => CommentItems?.Count ?? 0;
    protected int PendingCount => CountByStatus(ECommentStatus.Pending);
    protected int ApprovedCount => CountByStatus(ECommentStatus.Approved);
    protected int RejectedCount => CountByStatus(ECommentStatus.Rejected);

    protected List<CommentAdminOutput> FilteredComments => CommentItems?
        .Where(comment => !StatusFilter.HasValue || comment.Status == StatusFilter.Value)
        .Where(comment =>
            string.IsNullOrWhiteSpace(SearchTerm)
            || comment.AuthorName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
            || comment.Content.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
            || (comment.AuthorEmail?.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false))
        .OrderByDescending(comment => comment.CreatedAt)
        .ToList() ?? [];

    protected string DeleteConfirmationMessage => _commentPendingDeletion is null
        ? string.Empty
        : $"Deseja realmente excluir o comentário de ‘{_commentPendingDeletion.AuthorName}’? Esta ação não poderá ser desfeita.";

    protected override async Task OnInitializedAsync()
        => await LoadCommentsAsync();

    protected async Task ApproveAsync(Guid id)
        => await ChangeStatusAsync(id, approve: true);

    protected async Task RejectAsync(Guid id)
        => await ChangeStatusAsync(id, approve: false);

    protected void RequestDelete(Guid id)
    {
        _commentPendingDeletion = CommentItems?.FirstOrDefault(comment => comment.Id == id);
        ResetNotification();
    }

    protected void CancelDelete()
        => _commentPendingDeletion = null;

    protected async Task ConfirmDeleteAsync()
    {
        if (_commentPendingDeletion is null || IsDeleting)
            return;

        var comment = _commentPendingDeletion;
        IsDeleting = true;
        _processingCommentId = comment.Id;
        ResetNotification();

        try
        {
            await CommentService.DeleteAsync(comment.Id, CancellationToken.None);
            _commentPendingDeletion = null;
            await LoadCommentsAsync();
            NotificationMessage = "Comentário excluído com sucesso.";
        }
        catch (BaseException exception)
        {
            _commentPendingDeletion = null;
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao excluir o comentário {CommentId}.", comment.Id);
            _commentPendingDeletion = null;
            NotificationMessage = "Não foi possível excluir o comentário. Tente novamente.";
            NotificationIsError = true;
        }
        finally
        {
            IsDeleting = false;
            _processingCommentId = null;
        }
    }

    private async Task ChangeStatusAsync(Guid id, bool approve)
    {
        if (_processingCommentId.HasValue)
            return;

        _processingCommentId = id;
        ResetNotification();

        try
        {
            if (approve)
                await CommentService.ApproveAsync(id, CancellationToken.None);
            else
                await CommentService.RejectAsync(id, CancellationToken.None);

            await LoadCommentsAsync();
            NotificationMessage = approve
                ? "Comentário aprovado e publicado."
                : "Comentário rejeitado com sucesso.";
        }
        catch (BaseException exception)
        {
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao moderar o comentário {CommentId}.", id);
            NotificationMessage = "Não foi possível atualizar o comentário. Tente novamente.";
            NotificationIsError = true;
        }
        finally
        {
            _processingCommentId = null;
        }
    }

    private async Task LoadCommentsAsync()
        => CommentItems = await CommentService.GetAllAsync(CancellationToken.None);

    private int CountByStatus(ECommentStatus status)
        => CommentItems?.Count(comment => comment.Status == status) ?? 0;

    private void ResetNotification()
    {
        NotificationMessage = null;
        NotificationIsError = false;
    }
}
