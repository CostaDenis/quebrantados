using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Tags;
using Quebrantados.Web.Exceptions;
using Quebrantados.Web.Services.Tags;

namespace Quebrantados.Web.Components.Pages.Dashboard.Tags;

public partial class TagsPage : ComponentBase
{
    [Inject] public ITagService TagService { get; set; } = null!;
    [Inject] public ILogger<TagsPage> Logger { get; set; } = null!;

    protected List<TagListItem>? TagItems { get; set; }
    protected string? NotificationMessage { get; set; }
    protected bool NotificationIsError { get; set; }
    protected bool IsDeleting { get; set; }

    protected TagListItem? _tagPendingDeletion;

    protected string DeleteConfirmationMessage => _tagPendingDeletion is null
        ? string.Empty
        : $"Deseja realmente excluir a tag ‘{_tagPendingDeletion.Name}’? As reflexões serão mantidas, mas perderão a associação com ela.";

    protected int TagsInUseCount => TagItems?.Count(tag => tag.PostCount > 0) ?? 0;
    protected int UnusedTagsCount => TagItems?.Count(tag => tag.PostCount == 0) ?? 0;

    protected override async Task OnInitializedAsync()
        => TagItems = await TagService.GetAllAsync(CancellationToken.None);

    protected void RequestDelete(Guid id)
    {
        _tagPendingDeletion = TagItems?.FirstOrDefault(tag => tag.Id == id);
        ResetNotification();
    }

    protected void CancelDelete()
        => _tagPendingDeletion = null;

    protected async Task ConfirmDeleteAsync()
    {
        if (_tagPendingDeletion is null || IsDeleting)
            return;

        var tag = _tagPendingDeletion;
        IsDeleting = true;
        ResetNotification();

        try
        {
            await TagService.DeleteAsync(tag.Id, CancellationToken.None);
            TagItems?.Remove(tag);
            _tagPendingDeletion = null;
            NotificationMessage = $"Tag ‘{tag.Name}’ excluída com sucesso.";
        }
        catch (BaseException exception)
        {
            _tagPendingDeletion = null;
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao excluir a tag {TagId}.", tag.Id);
            _tagPendingDeletion = null;
            NotificationMessage = "Não foi possível excluir a tag. Tente novamente.";
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
