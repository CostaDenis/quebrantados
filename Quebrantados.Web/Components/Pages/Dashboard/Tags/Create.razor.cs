using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Tags;
using Quebrantados.Web.Exceptions;
using Quebrantados.Web.Services.Tags;

namespace Quebrantados.Web.Components.Pages.Dashboard.Tags;

public partial class CreatePage : ComponentBase
{
    [Inject] public ITagService TagService { get; set; } = null!;
    [Inject] public ILogger<CreatePage> Logger { get; set; } = null!;

    [SupplyParameterFromForm]
    public CreateTagInput Input { get; set; } = default!;

    protected string? NotificationMessage { get; set; }
    protected string? NotificationRedirectUrl { get; set; }
    protected bool NotificationIsError { get; set; }

    protected override void OnInitialized()
        => Input ??= new CreateTagInput();

    protected async Task Handle()
    {
        ResetNotification();

        try
        {
            await TagService.CreateAsync(Input, CancellationToken.None);
            NotificationMessage = "Tag adicionada com sucesso.";
            NotificationRedirectUrl = "/dashboard/tags";
        }
        catch (BaseException exception)
        {
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao criar a tag {TagName}.", Input.Name);
            NotificationMessage = "Não foi possível adicionar a tag. Tente novamente.";
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
