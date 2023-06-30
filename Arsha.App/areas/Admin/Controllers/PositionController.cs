using Arsha.App.Context;
using Arsha.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arsha.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly ArshaDbContext _context;

        public PositionController(ArshaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Position> Positions =
                await _context.Positions.Where(p => !p.IsDeleted).ToListAsync();
            return View(Positions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            position.CreatedDate = DateTime.Now;
            await _context.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "position");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Position? position = await _context.Positions.
                Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Position positionPost)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Position? position = await _context.Positions.
               Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
            if (position == null)
            {
                return NotFound();
            }
            position.UpdatedDate = DateTime.Now;
            position.Name = positionPost.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction("index", "position");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            Position? position = await _context.Positions.
             Where(p => !p.IsDeleted && p.Id == id).FirstOrDefaultAsync();
            if (position == null)
            {
                return NotFound();
            }

            position.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
