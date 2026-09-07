using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Dashboard;
using Quebrantados.Web.Services.Dashboard;

namespace Quebrantados.Web.Components.Pages.Dashboard;

public partial class DashboardPage : ComponentBase
{
    [Inject] public IDashboardService DashboardService { get; set; } = null!;

    protected DashboardOutput? DashboardOutput { get; set; }


    protected async override Task OnInitializedAsync()
        => DashboardOutput = await DashboardService.GetDataAsync(CancellationToken.None);

    public static string BuildPostNote(int publishedCount, int draftCount)
        => $"{publishedCount} {(publishedCount == 1 ? "publicada" : "publicadas")} e " +
           $"{draftCount} em rascunho";

    public static string BuildCategoryNote(int emptyCount) => emptyCount switch
    {
        0 => "Todas possuem conteúdo",
        1 => "1 categoria sem conteúdo",
        _ => $"{emptyCount} categorias sem conteúdo"
    };

    public static string BuildTagNote(int unusedCount) => unusedCount switch
    {
        0 => "Todas já foram utilizadas",
        1 => "1 tag ainda não foi utilizada",
        _ => $"{unusedCount} tags ainda não foram utilizadas"
    };

    public static string BuildDraftHeading(int draftCount)
        => draftCount == 1
            ? "Há 1 rascunho esperando por você."
            : $"Há {draftCount} rascunhos esperando por você.";
}
