namespace Quebrantados.Web.DTOs.Categories;

public class CategoryOutput(string name, string slug)
{
    public string Name { get; set; } = name;
    public string Slug { get; set; } = slug;
}