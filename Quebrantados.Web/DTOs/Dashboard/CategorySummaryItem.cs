namespace Quebrantados.Web.DTOs.Dashboard;

public class CategorySummaryItem(string name, int postCount, int percentage)
{
    public string Name { get; init; } = name;
    public int PostCount { get; init; } = postCount;
    public int Percentage { get; init; } = percentage;
}
