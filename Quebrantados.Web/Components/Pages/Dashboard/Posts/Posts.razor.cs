using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Posts;
using Quebrantados.Web.Enums;
using Quebrantados.Web.Exceptions;
using Quebrantados.Web.Services.Posts;

namespace Quebrantados.Web.Components.Pages.Dashboard.Posts;

public partial class PostsPage : ComponentBase
{
    [Inject] public IPostService PostService { get; set; } = null!;
    [Inject] public ILogger<PostsPage> Logger { get; set; } = null!;

    protected List<PostListItem>? PostItems { get; set; }
    protected string? NotificationMessage { get; set; }
    protected bool NotificationIsError { get; set; }
    protected bool IsDeleting { get; set; }
    protected PostListItem? _postPendingDeletion;

    protected int PublishedPostCount => PostItems?.Count(post => post.Status == EPostStatus.Published) ?? 0;
    protected int DraftPostCount => PostItems?.Count(post => post.Status == EPostStatus.Draft) ?? 0;
    protected int RecentlyUpdatedCount => PostItems?.Count(post => post.LastUpdateDate >= DateTime.UtcNow.AddDays(-7)) ?? 0;

    protected string DeleteConfirmationMessage => _postPendingDeletion is null
        ? string.Empty
        : $"Deseja realmente excluir a reflexão ‘{_postPendingDeletion.Title}’? Esta ação não poderá ser desfeita.";

    protected override async Task OnInitializedAsync()
        => PostItems = await PostService.GetAllAsync(CancellationToken.None);

    protected void RequestDelete(Guid id)
    {
        _postPendingDeletion = PostItems?.FirstOrDefault(post => post.Id == id);
        ResetNotification();
    }

    protected void CancelDelete()
        => _postPendingDeletion = null;

    protected async Task ConfirmDeleteAsync()
    {
        if (_postPendingDeletion is null || IsDeleting)
            return;

        var post = _postPendingDeletion;
        IsDeleting = true;
        ResetNotification();

        try
        {
            await PostService.DeleteAsync(post.Id, CancellationToken.None);
            PostItems?.Remove(post);
            _postPendingDeletion = null;
            NotificationMessage = $"Reflexão ‘{post.Title}’ excluída com sucesso.";
        }
        catch (BaseException exception)
        {
            _postPendingDeletion = null;
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao excluir a reflexão {PostId}.", post.Id);
            _postPendingDeletion = null;
            NotificationMessage = "Não foi possível excluir a reflexão. Tente novamente.";
            NotificationIsError = true;
        }
        finally
        {
            IsDeleting = false;
        }
    }

    private void ResetNotification()
    {
        NotificationMessage = null;
        NotificationIsError = false;
    }
}
