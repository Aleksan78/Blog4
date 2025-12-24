using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.Posts;
using Blog2.BLL.ViewModels.Tags;
using Blog2.DAL.Models;
using Blog2.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;



namespace Blog2.BLL.Services;

public class PostService(
    ITagRepository tagRepo,
    IPostRepository repo,
    IMapper mapper,
    UserManager<User> userManager,
    ICommentRepository commentRepo,
    ILogger<PostService> logger) : IPostService
{
    public async Task<PostCreateViewModel> CreatePost()
    {
        var dbTags = await tagRepo.GetAllTags();

        var model = new PostCreateViewModel
        {
            Title = string.Empty,
            Body = string.Empty,
            Tags = dbTags.Select(t => new TagViewModel { Id = t.Id, Name = t.Name }).ToList()
        };

        return model;
    }

    public async Task<Guid> CreatePost(PostCreateViewModel model)
    {
        var post = mapper.Map<Post>(model);
        post.CreatedData = DateTime.UtcNow;

        if (model.Tags != null)
        {
            var selectedTagIds = model.Tags.Where(t => t.IsSelected).Select(t => t.Id).ToList();
            var allDbTags = await tagRepo.GetAllTags();
            post.Tags = allDbTags.Where(t => selectedTagIds.Contains(t.Id)).ToList();
        }

        var user = await userManager.FindByIdAsync(model.AuthorId);
        if (user != null)
        {
            await repo.AddPost(post);
            logger.LogInformation("Пользователь {UserName} создал пост: {PostTitle}", user.UserName, post.Title);
        }

        return post.Id;
    }

    public async Task<PostEditViewModel?> EditPost(Guid id)
    {
        var post = await repo.GetPost(id);
        if (post == null) return null;

        var allDbTags = await tagRepo.GetAllTags();
        var tagsViewModel = allDbTags.Select(t => new TagViewModel
        {
            Id = t.Id,
            Name = t.Name,
            IsSelected = post.Tags.Any(pt => pt.Id == t.Id) // Компактная проверка выбора
        }).ToList();

        return new PostEditViewModel
        {
            id = id,
            Title = post.Title,
            Body = post.Body,
            Tags = tagsViewModel
        };
    }

    public async Task EditPost(PostEditViewModel model, Guid id)
    {
        var post = await repo.GetPost(id);
        if (post == null)
        {
            logger.LogWarning("Попытка редактирования несуществующего поста ID: {PostId}", id);
            return;
        }

        mapper.Map(model, post);

        // Обновление связей тегов
        post.Tags.Clear();
        if (model.Tags != null)
        {
            foreach (var tagVM in model.Tags.Where(t => t.IsSelected))
            {
                var dbTag = await tagRepo.GetTag(tagVM.Id);
                if (dbTag != null) post.Tags.Add(dbTag);
            }
        }

        await repo.UpdatePost(post);
        logger.LogInformation("Пост {PostId} отредактирован", post.Id);
    }

    public async Task RemovePost(Guid id)
    {
        var post = await repo.GetPost(id);
        if (post != null)
        {
            await repo.RemovePost(id);
            logger.LogInformation("Пост '{PostTitle}' (ID: {PostId}) удален", post.Title, id);
        }
    }

    public async Task<List<Post>> GetPosts() => await repo.GetAllPosts();

    public async Task<Post?> ShowPost(Guid id)
    {
        var post = await repo.GetPost(id);
        if (post == null) return null;

        var user = await userManager.FindByIdAsync(post.AuthorId);
        var comments = await commentRepo.GetCommentsByPostId(post.Id);

        // Чтобы не дублировать комментарии, просто присваиваем список, если репозиторий вернул актуальные
        post.Comments = comments;
        post.AuthorId = user?.UserName ?? "Anonymous";

        return post;
    }
}
