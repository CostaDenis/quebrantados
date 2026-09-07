using System.ComponentModel.DataAnnotations;

namespace Quebrantados.Web.DTOs.Posts;

public class CreatePostInput
{
    [Required(ErrorMessage = "Informe o título da reflexão!")]
    [StringLength(120, MinimumLength = 5, ErrorMessage = "O título deve possuir entre 5 e 120 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o slug!")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "O slug deve possuir entre 3 e 150 caracteres.")]
    public string Slug { get; set; } = string.Empty;

    [StringLength(200, MinimumLength = 20, ErrorMessage = "O resumo deve possuir entre 20 e 200 caracteres.")]
    public string? Summary { get; set; }

    [Required(ErrorMessage = "Escreva o conteúdo da reflexão!")]
    [StringLength(100_000, MinimumLength = 100, ErrorMessage = "O texto deve possuir entre 100 e 100.000 caracteres.")]
    public string Body { get; set; } = string.Empty;

    [Required(ErrorMessage = "Selecione uma categoria!")]
    public Guid CategoryId { get; set; }

    public List<Guid> TagIds { get; set; } = [];
}
