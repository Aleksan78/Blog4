using AutoMapper;
using Blog2.BLL.Services.IServices;
using Blog2.BLL.ViewModels.Tags;
using Blog2.DAL.Models;
using Blog2.DAL.Repositories.IRepositories;
using Microsoft.Extensions.Logging;

namespace Blog2.BLL.Services;

public class TagService(
    ITagRepository repo,
    IMapper mapper,
    ILogger<TagService> logger) : ITagService
{
    public async Task<Guid> CreateTag(TagCreateViewModel model)
    {
        var allTags = await repo.GetAllTags();

        if (allTags.Any(t => t.Name == model.Name))
        {
            logger.LogWarning("Попытка создания дубликата тега: {TagName}", model.Name);
            return Guid.Empty;
        }

        var tag = mapper.Map<Tag>(model);
        await repo.AddTag(tag);

        logger.LogInformation("Создан новый тег: {TagName} (ID: {TagId})", tag.Name, tag.Id);
        return tag.Id;
    }

    public async Task EditTag(TagEditViewModel model)
    {
        if (string.IsNullOrEmpty(model.Name)) return;

        var tag = await repo.GetTag(model.Id);
        if (tag == null)
        {
            logger.LogWarning("Тег ID: {TagId} не найден для редактирования", model.Id);
            return;
        }

        var oldName = tag.Name;
        var allTags = await repo.GetAllTags();

        if (allTags.Any(t => t.Name == model.Name && t.Id != model.Id))
        {
            logger.LogWarning("Имя {NewName} уже занято другим тегом", model.Name);
            return;
        }

        tag.Name = model.Name;
        await repo.UpdateTag(tag);

        logger.LogInformation("Тег {OldName} переименован в {NewName}", oldName, tag.Name);
    }

    public async Task RemoveTag(Guid id)
    {
        var tag = await repo.GetTag(id);
        if (tag != null)
        {
            await repo.RemoveTag(id);
            logger.LogInformation("Тег '{TagName}' (ID: {TagId}) удален", tag.Name, id);
        }
    }

    public async Task<List<Tag>> GetTags() => await repo.GetAllTags();
}