using SimpleBlog.Models;

namespace SimpleBlog.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetAllAsync();
        Task<Post> GetByIdAsync(int id);
        Task AddAsync(Post post, int[] tagIds);
        Task UpdateAsync(Post post, int[] tagIds);
        Task DeleteAsync(int id);
    }
}