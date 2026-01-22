using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamayaExam.Context;
using SamayaExam.Helper;
using SamayaExam.Models;
using SamayaExam.ViewModels.MemberViewModel;

namespace SamayaExam.Areas.Admin.Controllers;
[Area("Admin")]
//[Authorize(Roles ="Admin")]

public class MemberController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string _folderPath;

    public MemberController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _folderPath = Path.Combine(_environment.WebRootPath,"assets","img");
    }

    public async Task<IActionResult> Index()
    {
        var members = await _context.Members.Select(member => new MemberGetVM() 
        {
            Id=member.Id,
            Name=member.Name,
            ImagePath=member.ImagePath,
            CategoryName=member.Category.Title
        }).ToListAsync();

        return View(members);
    }

    public async Task<IActionResult> Create()
    {
        await SendCategoryWithViewBag();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(MemberCreateVM vm)
    {
        await SendCategoryWithViewBag();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        //Check categories
        var existCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
        if (!existCategory)
        {
            return NotFound();
        }
        //Check ImageSize
        if (!vm.Image.CheckSize(2))
        {
            ModelState.AddModelError("Image", "Image size must be less than 2mb");
            return View(vm);
        }
        //Check file type

        if (!vm.Image.Checktype("image"))
        {
            ModelState.AddModelError("Image", "File type must be image type ");
            return View(vm);
        }

        //Create image
        var uniqueFileName = await vm.Image.FileUploadToAsync(_folderPath);

        Member member = new() 
        {
            Name=vm.Name,
            CategoryId=vm.CategoryId,
            ImagePath=uniqueFileName
        };
        await _context.Members.AddAsync(member);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        await SendCategoryWithViewBag();
        var member = await _context.Members.FindAsync(id);
        if (member is null)
            return NotFound();
        MemberUpdateVM vm = new() 
        {
            Id=member.Id,
            Name=member.Name,
            CategoryId=member.CategoryId
        };
        return View(vm);

    }
    [HttpPost]
    public async Task<IActionResult> Update(MemberUpdateVM vm)
    {
        await SendCategoryWithViewBag();
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        //Find member for update
        var isExistMember = await _context.Members.FindAsync(vm.Id);
        if(isExistMember is null)
        {
            return NotFound();
        }

        //Check categories
        var existCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
        if (!existCategory)
        {
            return NotFound();
        }
        //Check ImageSize
        if (!vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("Image", "Image size must be less than 2mb");
            return View(vm);
        }
        //Check file type

        if (!vm.Image?.Checktype("image") ?? false)
        {
            ModelState.AddModelError("Image", "File type must be image type ");
            return View(vm);
        }
        //Update

        isExistMember.Name = vm.Name;
        isExistMember.CategoryId = vm.CategoryId;

        if(vm.Image is { })
        {
            string newFilePath = await vm.Image.FileUploadToAsync(_folderPath);
            string deletePath = Path.Combine(_folderPath, isExistMember.ImagePath);
            FileHelper.DeleteFile(deletePath);
            isExistMember.ImagePath = newFilePath;
        }

        _context.Members.Update(isExistMember);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member is null)
            return NotFound();
        _context.Members.Remove(member);
        await _context.SaveChangesAsync();

        //DELETE FILE

        var deletePath = Path.Combine(_folderPath, member.ImagePath);
        FileHelper.DeleteFile(deletePath);
        return RedirectToAction("Index");

    }
    private async Task SendCategoryWithViewBag()
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
    }
}
