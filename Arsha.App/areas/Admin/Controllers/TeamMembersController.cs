using Arsha.App.Context;
using Arsha.App.Extentions;
using Arsha.App.Helpers;
using Arsha.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Arsha.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamMembersController : Controller
    {

        private readonly ArshaDbContext _context;
        private IWebHostEnvironment _environment;

        public TeamMembersController(ArshaDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<TeamMembers> teamMembers =
                await _context.TeamMembers
                .Include(t=>t.SocialMedias).Include(t=>t.Position).
                Where(t => !t.IsDeleted).ToListAsync();
            return View(teamMembers);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Position = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamMembers teamMembers)
        {
            ViewBag.Position = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (teamMembers.File is null)
            {
                ModelState.AddModelError("file", "Image is required!");
                return View();
            }
            if (!Helper.isImage(teamMembers.File))
            {
                ModelState.AddModelError("file", "Image is required!");
                return View();
            }
            if (!Helper.isSizeOk(teamMembers.File, 1))
            {
                ModelState.AddModelError("file", "Image size is wrong!");
                return View();
            }

            teamMembers.CreatedDate=DateTime.Now;
            teamMembers.Position= _context.Positions.Where(x => x.Id == teamMembers.PositionId).FirstOrDefault();
            teamMembers.Image = teamMembers.File.CreateImage(_environment.WebRootPath, "assets/img/team");
            await _context.TeamMembers.AddAsync(teamMembers);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "TeamMembers");
        }
        public async Task<IActionResult> Update(int id)
        {
            TeamMembers? teamMember = await _context.TeamMembers.
                Where(t => !t.IsDeleted && t.Id == id).FirstOrDefaultAsync();


            ViewBag.Position = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();

            if (teamMember is null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, TeamMembers teamMember)
        {
            TeamMembers? updatedteamMember = await _context.
                TeamMembers.Where(t => !t.IsDeleted && t.Id == id).FirstOrDefaultAsync();


            ViewBag.Position = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();

            if (teamMember is null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(updatedteamMember);
            }

            if (teamMember.File != null)
            {
                if (!Helper.isImage(teamMember.File))
                {
                    ModelState.AddModelError("FormFile", "Wronggg!");
                    return View();
                }
                if (!Helper.isSizeOk(teamMember.File, 1))
                {
                    ModelState.AddModelError("FormFile", "Wronggg!");
                    return View();
                }

                updatedteamMember.Image = teamMember.File.CreateImage(_environment.WebRootPath, "assets/img/team");
            }

            updatedteamMember.UpdatedDate = DateTime.Now;
            updatedteamMember.FullName = teamMember.FullName;
            updatedteamMember.Description = teamMember.Description;
            updatedteamMember.PositionId = teamMember.PositionId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            TeamMembers? teamMember = await _context.TeamMembers.Where(t => !t.IsDeleted && t.Id == id).FirstOrDefaultAsync();
            if (teamMember is null)
            {
                return NotFound();
            }
            teamMember.IsDeleted = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
    }
