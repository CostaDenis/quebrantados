using Quebrantados.Web.DTOs.Dashboard;

namespace Quebrantados.Web.Services.Dashboard;

public interface IDashboardService
{
    Task<DashboardOutput> GetDataAsync(CancellationToken cancellationToken);
}