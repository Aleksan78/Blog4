using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.Comments;
using Blog2.DAL.Models;
using Blog2.DAL.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Blog2.BLL.Services; // Совет 4: File-scoped namespace

// Совет 4: Primary Constructor
public class CommentService(
    IMapper mapper,
    ICommentRepository commentRepo,
    UserManager<User> userManager,
    ILogger<CommentService> logger) : ICommentService
{
    public async Task<Guid> CreateComment(CommentCreateViewModel model, Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        var userName = user?.UserName ?? "Unknown";

        var comment = mapper.Map<Comment>(model);

        comment.AuthorId = userId;
        comment.realAuthorName = userName;
        comment.CreatedDate = DateTime.UtcNow;

        await commentRepo.AddComment(comment);

        // Совет 5: Логирование через параметры (лучше для поиска в будущем)
        logger.LogInformation("Пользователь {UserName} (ID: {UserId}) оставил комментарий к посту {PostId}",
            userName, userId, model.PostId);

        return comment.Id;
    }

    public async Task EditComment(CommentEditViewModel model)
    {
        var comment = await commentRepo.GetComment(model.Id);

        if (comment == null)
        {
            logger.LogWarning("Попытка редактирования несуществующего комментария ID: {CommentId}", model.Id);
            return;
        }

        comment.Title = model.Title;
        comment.Body = model.Description;
        comment.Author = model.Author;

        await commentRepo.UpdateComment(comment);

        logger.LogInformation("Комментарий {CommentId} отредактирован автором {AuthorName}",
            comment.Id, comment.realAuthorName);
    }

    public async Task RemoveComment(Guid id)
    {
        var comment = await commentRepo.GetComment(id);
        if (comment != null)
        {
            var authorName = comment.realAuthorName;
            await commentRepo.RemoveComment(id);

            logger.LogInformation("Комментарий {CommentId} (Автор: {Author}) успешно удален", id, authorName);
        }
        else
        {
            logger.LogWarning("Попытка удаления несуществующего комментария: {Id}", id);
        }
    }

    public async Task<List<Comment>> GetComments()
    {
        return await commentRepo.GetAllComments();
    }
}