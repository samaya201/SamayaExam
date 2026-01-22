using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamayaExam.Context;
using SamayaExam.ViewModels.MemberViewModel;


namespace SamayaExam.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {
        

        public async Task<IActionResult> IndexAsync()
        {
            var members = await _context.Members.Select(member => new MemberGetVM()
            {
                Id = member.Id,
                Name = member.Name,
                ImagePath = member.ImagePath,
                CategoryName = member.Category.Title
            }).ToListAsync();

            return View(members);
        }

        
    }
}
