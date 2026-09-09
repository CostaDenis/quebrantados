using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Categories;
using Quebrantados.Web.Exceptions;
using Quebrantados.Web.Services.Categories;

namespace Quebrantados.Web.Components.Pages.Dashboard.Categories;

public partial class EditorPage : ComponentBase
{
    [Inject] public ICategoryService CategoryService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ILogger<EditorPage> Logger { get; set; } = null!;

    [Parameter]
    public Guid Id { get; set; }

    [SupplyParameterFromForm]
    public EditCategoryInput Input { get; set; } = default!;

    protected string? NotificationMessage { get; set; }
    protected string? NotificationRedirectUrl { get; set; }
    protected bool NotificationIsError { get; set; }

    private bool _shouldLoadCategory;

    protected override void OnInitialized()
    {
        _shouldLoadCategory = Input is null;
        Input ??= new EditCategoryInput();
    }

    protected override async Task OnInitializedAsync()
    {
        if (!_shouldLoadCategory)
            return;

        var category = await CategoryService.GetByIdAsync(Id, CancellationToken.None);

        if (category is null)
        {
            NavigationManager.NavigateTo("/nao-encontrado");
            return;
        }

        Input = new EditCategoryInput
        {
            Name = category.Name,
            Slug = category.Slug
        };
    }

    protected async Task Handle()
    {
        ResetNotification();

        try
        {
            await CategoryService.UpdateAsync(Id, Input, CancellationToken.None);
            NotificationMessage = "Categoria atualizada com sucesso.";
            NotificationRedirectUrl = "/dashboard/categorias";
        }
        catch (BaseException exception)
        {
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao editar a categoria {CategoryId}.", Id);
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
