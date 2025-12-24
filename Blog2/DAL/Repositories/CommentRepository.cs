using Blog2.DAL.Models;
using Blog2.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Blog2.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly Blog2DbContext _context;

        public CommentRepository(Blog2DbContext context)
        {
            _context = context;
        }

        //public List<Comment> GetAllComments()
        public async Task<List<Comment>> GetAllComments()
        {
            //return _context.Comments.ToList();
            // Используем ToListAsync() из Microsoft.EntityFrameworkCore
            return await _context.Comments.ToListAsync();
        }

        //public Comment GetComment(Guid id)
        public async Task<Comment?> GetComment(Guid id)
        {
            //return _context.Comments.FirstOrDefault(c => c.Id == id);
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        //public List<Comment> GetCommentsByPostId(Guid id)
        public async Task<List<Comment>> GetCommentsByPostId(Guid id)
        {
            //return _context.Comments.Where(c => c.PostId == id).ToList();
            return await _context.Comments.Where(c => c.PostId == id).ToListAsync();
        }

        public async Task AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await SaveChangesAsync();
        }

        public async Task UpdateComment(Comment comment)
        {
            //_context.Comments.Update(comment);
            await _context.Comments.AddAsync(comment);
            await SaveChangesAsync();
        }

        public async Task RemoveComment(Guid id)
        {
            //_context.Comments.Remove(GetComment(id));
            var comment = await GetComment(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await SaveChangesAsync();
            }
            //await SaveChangesAsync();
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
