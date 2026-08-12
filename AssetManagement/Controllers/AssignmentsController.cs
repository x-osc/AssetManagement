using AssetManagement.Data;
using AssetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly AssetManagementContext _context;

        public AssignmentsController(AssetManagementContext context)
        {
            _context = context;
        }

        // GET: Assignments
        public async Task<IActionResult> Index()
        {
            var assetManagementContext = _context.Assignment.Include(a => a.Asset).Include(a => a.Location).Include(a => a.Person);
            return View(await assetManagementContext.ToListAsync());
        }

        // GET: Assignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment
                .Include(a => a.Asset)
                .Include(a => a.Location)
                .Include(a => a.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // GET: Assignments/Create
        public IActionResult Create(int? assetId, DateTime date, string? returnUrl)
        {
            ViewData["AssetId"] = new SelectList(_context.Asset, "Id", "Name");
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name");
            ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Name");

            var model = new Assignment();

            if (assetId != null)
            {
                model.AssetId = assetId.Value;
            }
            ViewData["Asset"] = _context.Asset.FirstOrDefault(a => a.Id == assetId);
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["Date"] = date;

            return View(model);
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssetId,PersonId,LocationId,AssignedAt,ReturnedAt,Notes")] Assignment assignment, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var asset = await _context.Asset
                    .Include(a => a.Assignments)
                    .FirstOrDefaultAsync(a => a.Id == assignment.AssetId);
                if (asset != null && asset.CurrentAssignment != null)
                {
                    ModelState.AddModelError(string.Empty, "This asset is already assigned.");
                    ViewData["AssetId"] = new SelectList(_context.Asset, "Id", "Name", assignment.AssetId);
                    ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name", assignment.LocationId);
                    ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Name", assignment.PersonId);
                    return View(assignment);
                }
                _context.Add(assignment);
                await _context.SaveChangesAsync();
                
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssetId"] = new SelectList(_context.Asset, "Id", "Name", assignment.AssetId);
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name", assignment.LocationId);
            ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Name", assignment.PersonId);
            return View(assignment);
        }

        public async Task<IActionResult> Return(int? id, string? returnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment
                .Include(a => a.Asset)
                .Include(a => a.Location)
                .Include(a => a.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(int id, string? returnUrl)
        {
            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }

            if (assignment.ReturnedAt == null)
            {
                assignment.ReturnedAt = DateTime.UtcNow;
                _context.Update(assignment);
                await _context.SaveChangesAsync();
            }

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Assignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }
            ViewData["AssetId"] = new SelectList(_context.Asset, "Id", "Name", assignment.AssetId);
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name", assignment.LocationId);
            ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Name", assignment.PersonId);
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AssetId,PersonId,LocationId,AssignedAt,ReturnedAt,Notes")] Assignment assignment)
        {
            if (id != assignment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(assignment.Id))
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
            ViewData["AssetId"] = new SelectList(_context.Asset, "Id", "Name", assignment.AssetId);
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name", assignment.LocationId);
            ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Name", assignment.PersonId);
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment
                .Include(a => a.Asset)
                .Include(a => a.Location)
                .Include(a => a.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment != null)
            {
                _context.Assignment.Remove(assignment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignment.Any(e => e.Id == id);
        }
    }
}
