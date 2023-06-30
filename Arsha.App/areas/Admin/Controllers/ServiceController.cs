using Arsha.App.Context;
using Arsha.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arsha.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly ArshaDbContext _context;

        public ServiceController(ArshaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Service> Services = await _context.Services.Where(s => !s.IsDeleted).ToListAsync();
            return View(Services);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "service");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
           Service? service= await _context.Services.Where(s => !s.IsDeleted && s.Id == id).FirstOrDefaultAsync();
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Service servicePost)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Service? service = await _context.Services.Where(s => !s.IsDeleted && s.Id == id).FirstOrDefaultAsync();
            if (service == null)
            {
                return NotFound();
            }
            service.Title = servicePost.Title;
            service.Description = servicePost.Description;
            service.Icon = servicePost.Icon;
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "service");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            Service? service = await _context.Services.Where(s => !s.IsDeleted && s.Id == id).FirstOrDefaultAsync();
            if (service == null)
            {
                return NotFound();
            }
            service.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
