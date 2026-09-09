using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Categories;
using Quebrantados.Web.Exceptions;
using Quebrantados.Web.Services.Categories;

namespace Quebrantados.Web.Components.Pages.Dashboard.Categories;

public partial class CreatePage : ComponentBase
{
    [Inject] public ICategoryService CategoryService { get; set; } = null!;
    [Inject] public ILogger<CreatePage> Logger { get; set; } = null!;

    [SupplyParameterFromForm]
    public CreateCategoryInput Input { get; set; } = default!;

    protected string? NotificationMessage { get; set; }
    protected string? NotificationRedirectUrl { get; set; }
    protected bool NotificationIsError { get; set; }

    protected override void OnInitialized()
        => Input ??= new CreateCategoryInput();

    protected async Task Handle()
    {
        ResetNotification();

        try
        {
            await CategoryService.CreateAsync(Input, CancellationToken.None);
            NotificationMessage = "Categoria adicionada com sucesso.";
            NotificationRedirectUrl = "/dashboard/categorias";
        }
        catch (BaseException exception)
        {
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao criar a categoria {CategoryName}.", Input.Name);
            NotificationMessage = "Não foi possível adicionar a categoria. Tente novamente.";
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
