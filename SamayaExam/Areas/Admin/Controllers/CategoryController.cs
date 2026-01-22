using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SamayaExam.Context;
using SamayaExam.Models;

namespace SamayaExam.Areas.Admin.Controllers;
[Area("Admin")]
//[Authorize(Roles ="Admin")]

public class CategoryController(AppDbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var category = await _context.Categories.ToListAsync();
        return View(category);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        Category newCategory = new() 
        {
            Title=category.Title
        };
        await _context.Categories.AddAsync(newCategory);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Update(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category is null)
            return NotFound();

        return View(category);
    }
    [HttpPost]
    public async Task<IActionResult> UpdateAsync(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }
        var isExistCategory = await _context.Categories.FindAsync(category.Id);
        if (isExistCategory is null)
            return NotFound();
        //Update
        isExistCategory.Title = category.Title;

        _context.Categories.Update(isExistCategory);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category is null)
            return NotFound();
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
