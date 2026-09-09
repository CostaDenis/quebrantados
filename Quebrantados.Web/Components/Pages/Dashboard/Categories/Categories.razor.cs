using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Categories;
using Quebrantados.Web.Exceptions;
using Quebrantados.Web.Services.Categories;

namespace Quebrantados.Web.Components.Pages.Dashboard.Categories;

public partial class CategoriesPage : ComponentBase
{
    [Inject] public ICategoryService CategoryService { get; set; } = null!;
    [Inject] public ILogger<CategoriesPage> Logger { get; set; } = null!;

    protected List<CategoryListItem>? CategoryItems { get; set; }
    protected string? NotificationMessage { get; set; }
    protected bool NotificationIsError { get; set; }
    protected bool IsDeleting { get; set; }
    protected CategoryListItem? _categoryPendingDeletion;

    protected int CategoriesInUseCount => CategoryItems?.Count(category => category.PostCount > 0) ?? 0;
    protected int EmptyCategoriesCount => CategoryItems?.Count(category => category.PostCount == 0) ?? 0;

    protected string DeleteConfirmationMessage => _categoryPendingDeletion is null
        ? string.Empty
        : $"Deseja realmente excluir a categoria ‘{_categoryPendingDeletion.Name}’? Esta ação não poderá ser desfeita.";

    protected override async Task OnInitializedAsync()
        => CategoryItems = await CategoryService.GetAllAsync(CancellationToken.None);

    protected void RequestDelete(Guid id)
    {
        _categoryPendingDeletion = CategoryItems?.FirstOrDefault(category => category.Id == id);
        ResetNotification();
    }

    protected void CancelDelete()
        => _categoryPendingDeletion = null;

    protected async Task ConfirmDeleteAsync()
    {
        if (_categoryPendingDeletion is null || IsDeleting)
            return;

        var category = _categoryPendingDeletion;
        IsDeleting = true;
        ResetNotification();

        try
        {
            await CategoryService.DeleteAsync(category.Id, CancellationToken.None);
            CategoryItems?.Remove(category);
            _categoryPendingDeletion = null;
            NotificationMessage = $"Categoria ‘{category.Name}’ excluída com sucesso.";
        }
        catch (BaseException exception)
        {
            _categoryPendingDeletion = null;
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao excluir a categoria {CategoryId}.", category.Id);
            _categoryPendingDeletion = null;
            NotificationMessage = "Não foi possível excluir a categoria. Tente novamente.";
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
