using Arsha.App.Context;
using Arsha.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arsha.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class SocialMediaController : Controller
    {

        private readonly ArshaDbContext _context;

        public SocialMediaController(ArshaDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<SocialMedia> socialMedias = await _context.SocialMedias.
                Include(x => x.TeamMembers).
                Where(x => !x.IsDeleted).ToListAsync();

            return View(socialMedias);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.TeamMembers = await _context.TeamMembers.Where(x => !x.IsDeleted).ToListAsync();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SocialMedia socialMedia)
        {
            ViewBag.TeamMembers = await _context.TeamMembers.Where(x => !x.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            socialMedia.CreatedDate = DateTime.Now;
            await _context.AddAsync(socialMedia);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "SocialMedia");
        }
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.TeamMembers = await _context.TeamMembers.Where(x => !x.IsDeleted).ToListAsync();

            SocialMedia? socialMedia = await _context.SocialMedias.
                Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (socialMedia is null)
            {
                return NotFound();
            }
            return View(socialMedia);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, SocialMedia socialMedia)
        {
            ViewBag.TeamMembers = await _context.TeamMembers.Where(x => !x.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            SocialMedia? updatedSocial = await _context.SocialMedias.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (socialMedia is null)
            {
                return NotFound();
            }
            updatedSocial.UpdatedDate = DateTime.Now;
            updatedSocial.TeamMembersId = socialMedia.TeamMembersId;
            updatedSocial.Icon = socialMedia.Icon;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            SocialMedia? social = await _context.SocialMedias.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (social is null)
            {
                return NotFound();
            }
            social.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
