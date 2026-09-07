using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Tags;
using Quebrantados.Web.Exceptions;
using Quebrantados.Web.Services.Tags;

namespace Quebrantados.Web.Components.Pages.Dashboard.Tags;

public partial class EditPage : ComponentBase
{
    [Inject] public ITagService TagService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ILogger<EditPage> Logger { get; set; } = null!;

    [Parameter]
    public Guid Id { get; set; }

    [SupplyParameterFromForm]
    public EditTagInput Input { get; set; } = default!;

    protected string? NotificationMessage { get; set; }
    protected string? NotificationRedirectUrl { get; set; }
    protected bool NotificationIsError { get; set; }

    private bool _shouldLoadTag;

    protected override void OnInitialized()
    {
        _shouldLoadTag = Input is null;
        Input ??= new EditTagInput();
    }

    protected override async Task OnInitializedAsync()
    {
        if (!_shouldLoadTag)
            return;

        var tag = await TagService.GetByIdAsync(Id, CancellationToken.None);

        if (tag is null)
        {
            NavigationManager.NavigateTo("/nao-encontrado");
            return;
        }

        Input = new EditTagInput
        {
            Name = tag.Name,
            Slug = tag.Slug
        };
    }

    protected async Task Handle()
    {
        ResetNotification();

        try
        {
            await TagService.UpdateAsync(Id, Input, CancellationToken.None);
            NotificationMessage = "Tag atualizada com sucesso.";
            NotificationRedirectUrl = "/dashboard/tags";
        }
        catch (BaseException exception)
        {
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao editar a tag {TagId}.", Id);
            NotificationMessage = "Não foi possível salvar as alterações. Tente novamente.";
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
