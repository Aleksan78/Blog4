using Blog2.DAL.Models;
using Blog2.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Blog2.DAL.Repositories
{
    public class TagRepository : ITagRepository
    {
        //private Blog2DbContext _context;
        private readonly Blog2DbContext _context; // Добавили readonly

        public TagRepository(Blog2DbContext context)
        {
            _context = context;
        }

        //public List<Tag> GetAllTags()
        //{
        //    return _context.Tags.ToList();
        //}
        public async Task<List<Tag>> GetAllTags()
        {
            return await _context.Tags.ToListAsync();
        }

        //public Tag GetTag(Guid id)
        //{
        //    return _context.Tags.FirstOrDefault(t => t.Id == id);
        //}
        public async Task<Tag?> GetTag(Guid id)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddTag(Tag tag)
        {
            //_context.Tags.Add(tag);
            await _context.Tags.AddAsync(tag);
            await SaveChangesAsync();
        }

        public async Task UpdateTag(Tag tag)
        {
            _context.Tags.Update(tag);
            await SaveChangesAsync();
        }

        public async Task RemoveTag(Guid id)
        {
            //_context.Tags.Remove(GetTag(id));
            //await SaveChangesAsync();
            var tag = await GetTag(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await SaveChangesAsync();
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            //if (await _context.SaveChangesAsync() > 0)
            //{
            //    return true;
            //}
            //return false;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
