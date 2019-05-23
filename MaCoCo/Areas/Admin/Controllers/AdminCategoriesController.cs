using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using MaCoCo.Data;
using MaCoCo.Models;

namespace MaCoCo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private string updateUrl = "http://supermaco.starwave.nl/api/categories";

        public AdminCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminCategories
        [Route("admin/categories")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
            //return View(GetSubcategories(0));
        }

        // GET: Admin/AdminCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.categoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/AdminCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("categoryId,name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/AdminCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("categoryId,name")] Category category)
        {
            if (id != category.categoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.categoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.categoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/AdminCategories/UpdateCategories
        [Route("admin/categories/update")]
        public async Task<IActionResult> UpdateFromAPI()
        {
            var rootElement = await Utilities.GetXML(updateUrl);
            List<Category> newCategories = new List<Category>();

            // Loop all categories
            foreach(var categoryElement in rootElement.Elements())
            {
                await HandleCategoryElement(categoryElement, 0, "Subcategory");
            }

            throw new Exception(newCategories.Count.ToString());
        }

        private async Task HandleCategoryElement(XElement element, int parentId, string subName)
        {
            Category currentCategory = new Category();

            // Get category name & subcategory tags
            foreach (var categoryPropertyElement in element.Elements())
            {
                if (categoryPropertyElement.Name == "Name")
                {
                    currentCategory.name = categoryPropertyElement.Value;
                    currentCategory.parentId = parentId;
                    _context.Add(currentCategory);
                    _context.SaveChanges();
                    continue;
                }

                if (categoryPropertyElement.Name == subName)
                {
                    // Recursion :D
                    await HandleCategoryElement(categoryPropertyElement, currentCategory.categoryId, "Sub" + subName.ToLower());
                }


            }
        }

        public List<Category> GetSubcategories(int categoryId)
        {
            return _context.Category.Where(item => item.parentId == categoryId).ToList();
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.categoryId == id);
        }
    }
}
