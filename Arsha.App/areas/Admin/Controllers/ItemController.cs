using Arsha.App.Context;
using Arsha.App.Extentions;
using Arsha.App.Helpers;
using Arsha.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arsha.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class ItemController : Controller
    {
        private readonly ArshaDbContext _context;
        private IWebHostEnvironment _evm;

        public ItemController(IWebHostEnvironment evm, ArshaDbContext context)
        {
            _evm = evm;
            _context = context;

        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Item> Items = await _context.Items.
                Include(x => x.Category).
                Where(x => !x.IsDeleted).ToListAsync();
            return View(Items);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Category = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {
            ViewBag.Category = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (item.File is null)
            {
                ModelState.AddModelError("file", "Image is required");
                return View();
            }
            if (!Helper.isImage(item.File))
            {
                ModelState.AddModelError("file", "Image is required");
                return View();
            }
            if (!Helper.isSizeOk(item.File, 1))
            {
                ModelState.AddModelError("file", "Image size is wrong");
                return View();
            }
            item.Category = await _context.Categories.Where(x => x.Id == item.CategoryId).FirstOrDefaultAsync();
            item.CreatedDate = DateTime.Now;
            item.Photo = item.File.CreateImage(_evm.WebRootPath, "assets/img/portfolio/");
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Item");
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Category = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            Item? item = await _context.Items.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (item is null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Item item)
        {
            ViewBag.Category = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            Item? UpdatedItem = await _context.Items.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (item is null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(UpdatedItem);
            }
            if (item.File is not null)
            {
                if (!Helper.isImage(item.File))
                {
                    ModelState.AddModelError("file", "Image is required");
                    return View();
                }
                if (!Helper.isSizeOk(item.File, 1))
                {
                    ModelState.AddModelError("file", "Image size is wrong");
                    return View();
                }
                UpdatedItem.Photo = item.File.CreateImage(_evm.WebRootPath, "assets/img/portfolio/");
            }

            UpdatedItem.UpdatedDate = DateTime.Now;
            UpdatedItem.Name = item.Name;
            UpdatedItem.CategoryId = item.CategoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Item? item = await _context.Items.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (item is null)
            {
                return NotFound();
            }
            item.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
