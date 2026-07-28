using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssetManagement.Data;
using AssetManagement.Models;
using AssetManagement.Common;

namespace AssetManagement.Controllers
{
    public class AssetsController : Controller
    {
        private readonly AssetManagementContext _context;

        public AssetsController(AssetManagementContext context)
        {
            _context = context;
        }

        public class AssetIndexViewModel
        {
            public List<Asset> Assets { get; set; } = [];
            public AssetFilter Filter { get; set; } = new AssetFilter();
            public int TotalItems { get; set; }
            public int TotalPages { get; set; }
        }

        public class AssetFilter
        {
            public string? Search { get; set; }
            public AssetStatus? Status { get; set; }
            public string? Sort { get; set; }
            public string? Order { get; set; }

            public int Page { get; set; } = 1;

            public string? GetNextOrder(string column)
            {
                return SortHelper.NextOrder(Sort, Order, column);
            }
        }

        // GET: Assets
        public async Task<IActionResult> Index(AssetFilter filter)
        {
            var query = _context.Asset.Include(a => a.Category).AsQueryable();

            if (!String.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(a => a.Name.Contains(filter.Search) || a.SerialNumber.Contains(filter.Search));
            }

            switch (filter.Sort) {
                case "name":
                    query = filter.Order == "desc" ? query.OrderByDescending(a => a.Name) : query.OrderBy(a => a.Name);
                    break;
                case "serial":
                    query = filter.Order == "desc" ? query.OrderByDescending(a => a.SerialNumber) : query.OrderBy(a => a.SerialNumber);
                    break;
                case "category":
                    query = filter.Order == "desc" ? query.OrderByDescending(a => a.Category.Name) : query.OrderBy(a => a.Category.Name);
                    break;
                case "status":
                    query = filter.Order == "desc" ? query.OrderByDescending(a => a.Status) : query.OrderBy(a => a.Status);
                    break;
                default:
                    query = filter.Order == "desc" ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id);
                    break;
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / 5.0);

            if (filter.Page < 1)
            {
                filter.Page = 1;
            }

            if (totalPages > 0 && filter.Page > totalPages)
            {
                filter.Page = totalPages;
            }

            query = query.Skip((filter.Page - 1) * 5).Take(5);

            var model = new AssetIndexViewModel
            {
                Assets = await query.ToListAsync(),
                Filter = filter,
                TotalItems = totalItems,
                TotalPages = totalPages,
            };

            return View(model);
        }

        // GET: Assets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Asset
                .Include(a => a.Category)
                .Include(a => a.Assignments)
                    .ThenInclude(a => a.Person)
                .Include(a => a.Assignments)
                    .ThenInclude(a => a.Location)
                .Include(a => a.MaintenanceLogs)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asset == null)
            {
                return NotFound();
            }

            return View(asset);
        }

        // GET: Assets/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View();
        }

        // POST: Assets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SerialNumber,Name,CategoryId,PurchaseDate,Notes")] Asset asset)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(asset);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", asset.CategoryId);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(asset);
        }

        // GET: Assets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Asset.FindAsync(id);
            if (asset == null)
            {   
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", asset.CategoryId);
            return View(asset);
        }

        // POST: Assets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Asset asset)
        {   
            if (id != asset.Id)
            {
                return NotFound();
            }

            var assetToUpdate = await _context.Asset.FirstOrDefaultAsync(m => m.Id == id);

            if (await TryUpdateModelAsync<Asset>(assetToUpdate, "", a => a.SerialNumber, a => a.Name, a => a.CategoryId, a => a.PurchaseDate, a => a.Notes))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssetExists(asset.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error occurred while updating asset: " + ex.Message);
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", assetToUpdate.CategoryId);
            return View(assetToUpdate);
        }

        // GET: Assets/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asset = await _context.Asset
                .AsNoTracking()
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (asset == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(asset);
        }

        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asset = await _context.Asset.FindAsync(id);
            if (asset == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Asset.Remove(asset);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            } catch (DbUpdateException ex) {
                Console.WriteLine("Error occurred while deleting asset: " + ex.Message);
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool AssetExists(int id)
        {
            return _context.Asset.Any(e => e.Id == id);
        }
    }
}
