using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AssetManagement.Data;
using AssetManagement.Models;

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
        public IActionResult Create()
        {
            ViewData["AssetId"] = new SelectList(_context.Asset, "Id", "Name");
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name");
            ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Name");
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssetId,PersonId,LocationId,AssignedAt,ReturnedAt,Notes")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssetId"] = new SelectList(_context.Asset, "Id", "Name", assignment.AssetId);
            ViewData["LocationId"] = new SelectList(_context.Set<Location>(), "Id", "Name", assignment.LocationId);
            ViewData["PersonId"] = new SelectList(_context.Set<Person>(), "Id", "Name", assignment.PersonId);
            return View(assignment);
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
