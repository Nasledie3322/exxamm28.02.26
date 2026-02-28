using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Models;
using SimpleBlog.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBlog.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly ITagService _tagService;

        public PostsController(IPostService postService, ITagService tagService)
        {
            _postService = postService;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _postService.GetAllAsync();
            return View(posts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Tags = await _tagService.GetAllAsync();
            ViewBag.SelectedTags = new int[] { };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Post post, int[] selectedTags)
        {
            if (ModelState.IsValid)
            {
                post.PublishedAt = DateTime.UtcNow;
                await _postService.AddAsync(post, selectedTags);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Tags = await _tagService.GetAllAsync();
            ViewBag.SelectedTags = selectedTags;
            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null) return NotFound();
            var allTags = await _tagService.GetAllAsync();
            ViewBag.Tags = allTags;
            ViewBag.SelectedTags = post.PostTags.Select(pt => pt.TagId).ToArray();
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Post post, int[] selectedTags)
        {
            if (ModelState.IsValid)
            {
                post.PublishedAt = post.PublishedAt.ToUniversalTime();
                await _postService.UpdateAsync(post, selectedTags);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Tags = await _tagService.GetAllAsync();
            ViewBag.SelectedTags = selectedTags;
            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _postService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}