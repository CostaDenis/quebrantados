using System.Globalization;
using Microsoft.AspNetCore.Components;
using Quebrantados.Web.DTOs.Comments;
using Quebrantados.Web.DTOs.Posts;
using Quebrantados.Web.Enums;
using Quebrantados.Web.Exceptions;
using Quebrantados.Web.Services.Categories;
using Quebrantados.Web.Services.Comments;
using Quebrantados.Web.Services.Posts;
using Quebrantados.Web.Services.Tags;

namespace Quebrantados.Web.Components.Pages;

public partial class PostPageBase : ComponentBase
{
    [Inject] public IPostService PostService { get; set; } = null!;
    [Inject] public ICategoryService CategoryService { get; set; } = null!;
    [Inject] public ITagService TagService { get; set; } = null!;
    [Inject] public ICommentService CommentService { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public ILogger<PostPageBase> Logger { get; set; } = null!;

    [Parameter]
    public string Slug { get; set; } = string.Empty;

    protected PostOutput Post { get; set; } = null!;
    protected string CategorySlug { get; set; } = string.Empty;
    protected List<PostTagView> Tags { get; set; } = [];
    protected List<PostListItem> RelatedPosts { get; set; } = [];
    protected List<CommentOutput> Comments { get; set; } = [];
    protected CommentInput CommentForm { get; set; } = new();
    protected string? NotificationMessage { get; set; }
    protected bool NotificationIsError { get; set; }
    protected bool IsSubmittingComment { get; set; }

    protected string PublishedIsoDate => PublicationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    protected string PublishedDate => PublicationDate.ToString("dd MMM yyyy", BrazilianCulture);
    protected string ReadTime => $"{Math.Max(1, (int)Math.Ceiling(WordCount / 200d))} min";
    protected string CommentCountText => Comments.Count == 1
        ? "1 comentário"
        : $"{Comments.Count} comentários";

    private static CultureInfo BrazilianCulture => CultureInfo.GetCultureInfo("pt-BR");
    private DateTime PublicationDate => Post.PublishedAt ?? Post.CreatedAt;
    private int WordCount => Post.Body.Split(
        [' ', '\r', '\n', '\t'],
        StringSplitOptions.RemoveEmptyEntries).Length;

    protected override async Task OnParametersSetAsync()
    {
        var post = await PostService.GetBySlugAsync(Slug, CancellationToken.None);

        if (post is null || post.Status != EPostStatus.Published)
        {
            NavigationManager.NavigateTo("/nao-encontrado");
            return;
        }

        Post = post;
        CommentForm.PostId = post.Id;
        Comments = await CommentService.GetApprovedByPostIdAsync(post.Id, CancellationToken.None);

        var category = await CategoryService.GetByIdAsync(post.CategoryId, CancellationToken.None);
        CategorySlug = category?.Slug ?? string.Empty;

        Tags = [];

        foreach (var tagId in post.TagIds)
        {
            var tag = await TagService.GetByIdAsync(tagId, CancellationToken.None);

            if (tag is not null)
                Tags.Add(new PostTagView(tag.Name, tag.Slug));
        }

        var allPosts = await PostService.GetAllAsync(CancellationToken.None);
        RelatedPosts = allPosts
            .Where(item =>
                item.Id != post.Id
                && item.Status == EPostStatus.Published
                && item.CategoryName == post.CategoryName)
            .OrderByDescending(item => item.LastUpdateDate)
            .Take(3)
            .ToList();
    }

    protected async Task SubmitCommentAsync()
    {
        if (IsSubmittingComment)
            return;

        IsSubmittingComment = true;
        ResetNotification();

        try
        {
            await CommentService.CreateAsync(CommentForm, CancellationToken.None);
            CommentForm = new CommentInput { PostId = Post.Id };
            NotificationMessage = "Comentário enviado para moderação. Obrigado por participar!";
        }
        catch (BaseException exception)
        {
            NotificationMessage = exception.Message;
            NotificationIsError = true;
        }
        catch (Exception exception)
        {
            Logger.LogError(exception, "Erro ao enviar comentário para a reflexão {PostId}.", Post.Id);
            NotificationMessage = "Não foi possível enviar seu comentário. Tente novamente.";
            NotificationIsError = true;
        }
        finally
        {
            IsSubmittingComment = false;
        }
    }

    protected static string FormatCommentDate(DateTime date)
        => date.ToString("dd MMM yyyy", BrazilianCulture);

    private void ResetNotification()
    {
        NotificationMessage = null;
        NotificationIsError = false;
    }

    protected sealed record PostTagView(string Name, string Slug);
}
