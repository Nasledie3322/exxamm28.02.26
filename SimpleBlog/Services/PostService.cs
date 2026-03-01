using SimpleBlog.Models;
using SimpleBlog.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlog.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _db;

        public PostService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            var posts = await _db.Posts.Include(p => p.PostTags).ThenInclude(pt => pt.Tag).ToListAsync();
            return posts ?? new List<Post>();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _db.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Post post, int[]? tagIds)
        {
            post.PostTags = (tagIds ?? new int[0]).Select(tid => new PostTag { TagId = tid }).ToList();
            _db.Posts.Add(post);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Post post, int[]? tagIds)
        {
            var existing = await _db.Posts.Include(p => p.PostTags).FirstOrDefaultAsync(p => p.Id == post.Id);
            if (existing == null) return;
            existing.Title = post.Title;
            existing.Content = post.Content;
            existing.PublishedAt = post.PublishedAt;
            existing.PostTags.Clear();
            foreach (var tid in tagIds ?? new int[0])
            {
                existing.PostTags.Add(new PostTag { TagId = tid });
            }
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var post = await _db.Posts.FindAsync(id);
            if (post != null)
            {
                _db.Posts.Remove(post);
                await _db.SaveChangesAsync();
            }
        }
    }
}