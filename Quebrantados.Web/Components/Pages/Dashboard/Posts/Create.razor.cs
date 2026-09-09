using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Categories;
using Quebrantados.Web.DTOs.Posts;
using Quebrantados.Web.DTOs.Tags;
using Quebrantados.Web.Exceptions;
using Quebrantados.Web.Services.Categories;
using Quebrantados.Web.Services.Posts;
using Quebrantados.Web.Services.Tags;

namespace Quebrantados.Web.Components.Pages.Dashboard.Posts;

public partial class CreatePage : ComponentBase
{
    [Inject] public IPostService PostService { get; set; } = null!;
    [Inject] public ICategoryService CategoryService { get; set; } = null!;
    [Inject] public ITagService TagService { get; set; } = null!;
    [Inject] public ILogger<CreatePage> Logger { get; set; } = null!;

    [SupplyParameterFromForm]
    public CreatePostInput Input { get; set; } = default!;

    protected List<CategoryListItem> Categories { get; set; } = [];
    protected List<TagListItem> Tags { get; set; } = [];
    protected string? NotificationMessage { get; set; }
    protected string? NotificationRedirectUrl { get; set; }
    protected bool NotificationIsError { get; set; }

    private bool _publishRequested;

    protected override void OnInitialized()
        => Input ??= new CreatePostInput();

    protected override async Task OnInitializedAsync()
    {
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
            await PostService.CreateAsync(Input, _publishRequested, CancellationToken.None);
            NotificationMessage = _publishRequested
                ? "Reflexão publicada com sucesso."
                : "Rascunho salvo com sucesso.";
            NotificationRedirectUrl = "/dashboard/reflexoes";
        }
        catch (BaseException exception)
        {
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao criar a reflexão {PostTitle}.", Input.Title);
            NotificationMessage = "Não foi possível salvar a reflexão. Tente novamente.";
            NotificationIsError = true;
        }
    }

    private void ResetNotification()
    {
        NotificationMessage = null;
        NotificationRedirectUrl = null;
        NotificationIsError = false;
    }
}
