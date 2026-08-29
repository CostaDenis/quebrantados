using System.ComponentModel.DataAnnotations;

namespace Quebrantados.Web.Features.Categories;

public class EditCategoryInput
{
    [Required(ErrorMessage = "Informe o nome da categoria!")]
    [StringLength(60, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o slug!")]
    [StringLength(150,
        MinimumLength = 3,
        ErrorMessage = "O slug deve possuir entre 3 e 150 caracteres.")]
    public string Slug { get; set; } = string.Empty;
}