using Microsoft.EntityFrameworkCore;
using SimpleBlog.Data;
using SimpleBlog.Models;

namespace SimpleBlog.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _db;
        public TagService(ApplicationDbContext db) => _db = db;

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _db.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _db.Tags.FindAsync(id);
        }

        public async Task AddAsync(Tag tag)
        {
            _db.Tags.Add(tag);
            await _db.SaveChangesAsync();
        }
    }
}