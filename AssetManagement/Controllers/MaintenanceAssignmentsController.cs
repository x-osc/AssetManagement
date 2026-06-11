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
    public class MaintenanceAssignmentsController : Controller
    {
        private readonly AssetManagementContext _context;

        public MaintenanceAssignmentsController(AssetManagementContext context)
        {
            _context = context;
        }

        // GET: MaintenanceAssignments
        public async Task<IActionResult> Index()
        {
            var assetManagementContext = _context.MaintenanceAssignment.Include(m => m.Asset).Include(m => m.Technician);
            return View(await assetManagementContext.ToListAsync());
        }

        // GET: MaintenanceAssignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceAssignment = await _context.MaintenanceAssignment
                .Include(m => m.Asset)
                .Include(m => m.Technician)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (maintenanceAssignment == null)
            {
                return NotFound();
            }

            return View(maintenanceAssignment);
        }

        // GET: MaintenanceAssignments/Create
        public IActionResult Create()
        {
            ViewData["AssetId"] = new SelectList(_context.Asset, "Id", "Id");
            ViewData["TechnicianId"] = new SelectList(_context.Set<Person>(), "Id", "Id");
            return View();
        }

        // POST: MaintenanceAssignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartedAt,CompletedAt,Notes,AssetId,TechnicianId")] MaintenanceAssignment maintenanceAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(maintenanceAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssetId"] = new SelectList(_context.Asset, "Id", "Id", maintenanceAssignment.AssetId);
            ViewData["TechnicianId"] = new SelectList(_context.Set<Person>(), "Id", "Id", maintenanceAssignment.TechnicianId);
            return View(maintenanceAssignment);
        }

        // GET: MaintenanceAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceAssignment = await _context.MaintenanceAssignment.FindAsync(id);
            if (maintenanceAssignment == null)
            {
                return NotFound();
            }
            ViewData["AssetId"] = new SelectList(_context.Asset, "Id", "Id", maintenanceAssignment.AssetId);
            ViewData["TechnicianId"] = new SelectList(_context.Set<Person>(), "Id", "Id", maintenanceAssignment.TechnicianId);
            return View(maintenanceAssignment);
        }

        // POST: MaintenanceAssignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartedAt,CompletedAt,Notes,AssetId,TechnicianId")] MaintenanceAssignment maintenanceAssignment)
        {
            if (id != maintenanceAssignment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(maintenanceAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaintenanceAssignmentExists(maintenanceAssignment.Id))
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
            ViewData["AssetId"] = new SelectList(_context.Asset, "Id", "Id", maintenanceAssignment.AssetId);
            ViewData["TechnicianId"] = new SelectList(_context.Set<Person>(), "Id", "Id", maintenanceAssignment.TechnicianId);
            return View(maintenanceAssignment);
        }

        // GET: MaintenanceAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maintenanceAssignment = await _context.MaintenanceAssignment
                .Include(m => m.Asset)
                .Include(m => m.Technician)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (maintenanceAssignment == null)
            {
                return NotFound();
            }

            return View(maintenanceAssignment);
        }

        // POST: MaintenanceAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var maintenanceAssignment = await _context.MaintenanceAssignment.FindAsync(id);
            if (maintenanceAssignment != null)
            {
                _context.MaintenanceAssignment.Remove(maintenanceAssignment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaintenanceAssignmentExists(int id)
        {
            return _context.MaintenanceAssignment.Any(e => e.Id == id);
        }
    }
}
