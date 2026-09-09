using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Categories;
using Quebrantados.Web.DTOs.Posts;
using Quebrantados.Web.DTOs.Tags;
using Quebrantados.Web.Enums;
using Quebrantados.Web.Exceptions;
using Quebrantados.Web.Services.Categories;
using Quebrantados.Web.Services.Posts;
using Quebrantados.Web.Services.Tags;

namespace Quebrantados.Web.Components.Pages.Dashboard.Posts;

public partial class EditPage : ComponentBase
{
    [Inject] public IPostService PostService { get; set; } = null!;
    [Inject] public ICategoryService CategoryService { get; set; } = null!;
    [Inject] public ITagService TagService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ILogger<EditPage> Logger { get; set; } = null!;

    [Parameter]
    public Guid Id { get; set; }

    [SupplyParameterFromForm]
    public EditPostInput Input { get; set; } = default!;

    protected List<CategoryListItem> Categories { get; set; } = [];
    protected List<TagListItem> Tags { get; set; } = [];
    protected string? NotificationMessage { get; set; }
    protected string? NotificationRedirectUrl { get; set; }
    protected bool NotificationIsError { get; set; }
    protected bool IsDeleteConfirmationOpen { get; set; }
    protected bool IsDeleting { get; set; }

    private EPostStatus _currentStatus;
    private DateTime _lastUpdateDate;
    private bool _publishRequested;
    private bool _shouldLoadPost;

    protected string LastUpdateText => _lastUpdateDate == default
        ? "carregando..."
        : _lastUpdateDate.ToString("dd/MM/yyyy 'às' HH:mm");

    protected string StatusText => _currentStatus == EPostStatus.Published
        ? "Publicada"
        : "Rascunho";

    protected string StatusClasses => _currentStatus == EPostStatus.Published
        ? "mt-2 inline-flex rounded-full bg-green-50 px-3 py-1 text-xs font-semibold text-green-700"
        : "mt-2 inline-flex rounded-full bg-amber-50 px-3 py-1 text-xs font-semibold text-amber-700";

    protected string StatusDescription => _currentStatus == EPostStatus.Published
        ? "Esta reflexão está visível para os leitores. Você pode atualizar o conteúdo ou movê-la novamente para rascunho."
        : "Esta reflexão ainda não está visível publicamente. Revise o conteúdo antes de publicá-la.";

    protected string DeleteConfirmationMessage
        => $"Deseja realmente excluir a reflexão ‘{Input.Title}’? Esta ação não poderá ser desfeita.";

    protected override void OnInitialized()
    {
        _shouldLoadPost = Input is null;
        Input ??= new EditPostInput();
    }

    protected override async Task OnInitializedAsync()
    {
        if (!_shouldLoadPost)
            return;

        var post = await PostService.GetByIdAsync(Id, CancellationToken.None);

        if (post is null)
        {
            NavigationManager.NavigateTo("/nao-encontrado");
            return;
        }

        Input = new EditPostInput
        {
            Title = post.Title,
            Slug = post.Slug,
            Summary = post.Summary,
            Body = post.Body,
            CategoryId = post.CategoryId,
            TagIds = [.. post.TagIds]
        };

        _currentStatus = post.Status;
        _lastUpdateDate = post.LastUpdateDate;

        Categories = await CategoryService.GetAllAsync(CancellationToken.None);
        Tags = await TagService.GetAllAsync(CancellationToken.None);
    }

    protected void ToggleTag(Guid tagId, ChangeEventArgs eventArgs)
    {
        var selected = eventArgs.Value is true;

        if (selected && !Input.TagIds.Contains(tagId))
            Input.TagIds.Add(tagId);
        else if (!selected)
            Input.TagIds.Remove(tagId);
    }

    protected void SaveAsDraft()
        => _publishRequested = false;

    protected void Publish()
        => _publishRequested = true;

    protected async Task Handle()
    {
        ResetNotification();

        try
        {
            await PostService.UpdateAsync(Id, Input, CancellationToken.None);

            if (_publishRequested && _currentStatus == EPostStatus.Draft)
                await PostService.PublishAsync(Id, CancellationToken.None);
            else if (!_publishRequested && _currentStatus == EPostStatus.Published)
                await PostService.MoveToDraftAsync(Id, CancellationToken.None);

            NotificationMessage = _publishRequested
                ? "Reflexão salva e publicada com sucesso."
                : "Reflexão salva como rascunho.";
            NotificationRedirectUrl = "/dashboard/reflexoes";
        }
        catch (BaseException exception)
        {
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao editar a reflexão {PostId}.", Id);
            NotificationMessage = "Não foi possível salvar as alterações. Tente novamente.";
            NotificationIsError = true;
        }
    }

    protected void RequestDelete()
    {
        ResetNotification();
        IsDeleteConfirmationOpen = true;
    }

    protected void CancelDelete()
        => IsDeleteConfirmationOpen = false;

    protected async Task ConfirmDeleteAsync()
    {
        if (IsDeleting)
            return;

        IsDeleting = true;
        ResetNotification();

        try
        {
            await PostService.DeleteAsync(Id, CancellationToken.None);
            IsDeleteConfirmationOpen = false;
            NotificationMessage = "Reflexão excluída com sucesso.";
            NotificationRedirectUrl = "/dashboard/reflexoes";
        }
        catch (BaseException exception)
        {
            IsDeleteConfirmationOpen = false;
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao excluir a reflexão {PostId}.", Id);
            IsDeleteConfirmationOpen = false;
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
        NotificationRedirectUrl = null;
        NotificationIsError = false;
    }
}
