using System.ComponentModel.DataAnnotations;

namespace Quebrantados.Web.DTOs.Comments;

public class CommentInput : IValidatableObject
{
    public Guid PostId { get; set; }

    [Required(ErrorMessage = "Informe seu nome!")]
    [StringLength(
        80,
        MinimumLength = 2,
        ErrorMessage = "O nome deve possuir entre 2 e 80 caracteres!")]
    public string AuthorName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Informe um endereço de e-mail válido!")]
    [StringLength(
        255,
        ErrorMessage = "O e-mail deve possuir no máximo 255 caracteres.")]
    public string? AuthorEmail { get; set; }

    [Required(ErrorMessage = "Escreva seu comentário!")]
    [StringLength(
        1000,
        MinimumLength = 3,
        ErrorMessage = "O comentário deve possuir entre 3 e 1.000 caracteres.")]
    public string Content { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PostId == Guid.Empty)
        {
            yield return new ValidationResult(
                "Não foi possível identificar a reflexão.",
                [nameof(PostId)]);
        }
    }
}
