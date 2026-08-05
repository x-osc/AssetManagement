using AssetManagement.Data;
using AssetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AssetManagementContext _context;
        public DashboardController(AssetManagementContext context)
        {
            _context = context;
        }

        public class DashboardViewModel
        {
            public int TotalAssets { get; set; }

            public int AvailableAssets { get; set; }
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                TotalAssets = await _context.Asset.CountAsync(),
                AvailableAssets = await _context.Asset.CountAsync(a => a.Status == AssetStatus.Available),
            };

            return View(model);
        }
    }
}
