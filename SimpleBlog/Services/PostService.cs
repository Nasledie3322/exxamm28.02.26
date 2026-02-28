using Microsoft.EntityFrameworkCore;
using SimpleBlog.Data;
using SimpleBlog.Models;

namespace SimpleBlog.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _db;
        public PostService(ApplicationDbContext db) => _db = db;

        public async Task<List<Post>> GetAllAsync()
        {
            return await _db.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .ToListAsync();
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            return await _db.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Post post, int[] tagIds)
        {
            foreach (var tagId in tagIds)
                post.PostTags.Add(new PostTag { TagId = tagId });

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Post post, int[] tagIds)
        {
            var existing = await _db.Posts.Include(p => p.PostTags)
                                          .FirstOrDefaultAsync(p => p.Id == post.Id);
            if (existing == null) return;

            existing.Title = post.Title;
            existing.Content = post.Content;

            existing.PostTags.Clear();
            foreach (var tagId in tagIds)
                existing.PostTags.Add(new PostTag { TagId = tagId });

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post == null) return;
            _db.Posts.Remove(post);
            await _db.SaveChangesAsync();
        }
    }
}