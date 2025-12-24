using Blog2.DAL.Models;
using Blog2.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Blog2.DAL.Repositories
{
    public class PostRepository : IPostRepository
    {
        //private Blog2DbContext _context;
        private readonly Blog2DbContext _context;

        public PostRepository(Blog2DbContext context)
        {
            _context = context;
        }

        //public List<Post> GetAllPosts()
        //{
        //    return _context.Posts.Include(p => p.Tags).ToList();
        //}
        public async Task<List<Post>> GetAllPosts()
        {
            // Подгружаем теги сразу для списка статей
            return await _context.Posts
                .Include(p => p.Tags)
                .OrderByDescending(p => p.CreatedData) // Хорошая практика: свежие сверху
                .ToListAsync();
        }

        //public Post GetPost(Guid id)
        //{
        //    return _context.Posts.Include(p => p.Tags).FirstOrDefault(p => p.Id == id);
        //}
        public async Task<Post?> GetPost(Guid id)
        {
            return await _context.Posts
                .Include(p => p.Tags)
                .Include(p => p.Comments) // Важно: подгружаем комментарии для страницы статьи
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPost(Post post)
        {
            //_context.Posts.Add(post);
            await _context.Posts.AddAsync(post);
            await SaveChangesAsync();
        }

        public async Task UpdatePost(Post post)
        {
            _context.Posts.Update(post);
            await SaveChangesAsync();
        }

        public async Task RemovePost(Guid id)
        {
            //var post = GetPost(id)
            // Используем асинхронный поиск внутри репозитория
            var post = await GetPost(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
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
