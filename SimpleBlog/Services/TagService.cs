using SimpleBlog.Models;
using SimpleBlog.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBlog.Services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _db;

        public TagService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _db.Tags.ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            return await _db.Tags.FindAsync(id);
        }

        public async Task AddAsync(Tag tag)
        {
            _db.Tags.Add(tag);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag tag)
        {
            var existing = await _db.Tags.FindAsync(tag.Id);
            if (existing == null) return;
            existing.Name = tag.Name;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var tag = await _db.Tags.FindAsync(id);
            if (tag != null)
            {
                _db.Tags.Remove(tag);
                await _db.SaveChangesAsync();
            }
        }
    }
}