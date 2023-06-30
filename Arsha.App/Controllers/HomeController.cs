
using Arsha.App.Context;
using Arsha.App.ViewModels;
using Arsha.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Arsha.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ArshaDbContext _context;
        public HomeController(ArshaDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM();
           
            homeVM.Categories = await _context.Categories.Where(c => !c.IsDeleted)
                .ToListAsync();
            homeVM.Services = await _context.Services.Where(s => !s.IsDeleted)
            .ToListAsync();
           homeVM.TeamMembers= await _context.TeamMembers.Include(x=>x.SocialMedias).
                Include(x=>x.Position).Where(x=>!x.IsDeleted).ToListAsync();

            homeVM.Items = await _context.Items.Include(x => x.Category).
            Where(x => !x.IsDeleted).ToListAsync();
            return View(homeVM);
        }
    }

}