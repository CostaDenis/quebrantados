using System.ComponentModel.DataAnnotations;

namespace Quebrantados.Web.Features.Tags;

public class CreateTagInput
{
    [Required(ErrorMessage = "Informe o nome da tag!")]
    [StringLength(
        40,
        MinimumLength = 2,
        ErrorMessage = "O nome deve possuir entre 2 e 40 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o slug!")]
    [StringLength(
        150,
        MinimumLength = 2,
        ErrorMessage = "O slug deve possuir entre 2 e 150 caracteres.")]
    public string Slug { get; set; } = string.Empty;
}
