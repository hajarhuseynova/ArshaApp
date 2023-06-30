using Arsha.App.Context;
using Arsha.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arsha.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ArshaDbContext _context;

        public CategoryController(ArshaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> Categories =
                await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            return View(Categories);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            category.CreatedDate = DateTime.Now;
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "category");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Category? category = await _context.Categories.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Category categoryPost)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Category? category = await _context.Categories.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }
            category.UpdatedDate = DateTime.Now;    
            category.Name = categoryPost.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "category");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Category? category = await _context.Categories.Where(c => !c.IsDeleted && c.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return NotFound();
            }
            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

 }
}
