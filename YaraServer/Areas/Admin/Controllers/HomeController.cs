using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YaraServer.Data;
using YaraServer.Models;
using YaraServer.Models.ViewModels;

namespace YaraServer.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            int totalScans = _db.Reports.Count();
            int potentialMalware = _db.Reports.Where(r => r.Tag > 0).Count();
            int totalTerminals = _db.Terminals.Count();

            var yaraMatches = await _db.YaraResults.GroupBy(l => l.Identifier, l => l, (key, g) => new HomeViewModel.DictObject { Key = key, Count = g.Count() }).ToListAsync();
            var scans = await _db.Scans.Where(s => s.Result != null).GroupBy(l => l.Result, l => l, (key, g) => new HomeViewModel.DictObject { Key = key, Count = g.Count() }).ToListAsync();
            var reports = await _db.Reports.Where(s => s.Tag != 0).GroupBy(l => l.Date.Date, l => l, (key, g) => new HomeViewModel.DictObject { Key = key.ToString(), Count = g.Count() }).ToListAsync();
            var scansPerDay = await _db.Reports.GroupBy(l => l.Date.Date, l => l, (key, g) => new HomeViewModel.DictObject { Key = key.Date.ToString(), Count = g.Count() }).ToListAsync();

            HomeViewModel homeViewModel = new HomeViewModel()
            {
                TotalScans = totalScans,
                TotalPotentialInfected = potentialMalware,
                TotalTerminals = totalTerminals,
                YaraResults = yaraMatches,
                Scans = scans,
                Reports = reports,
                ScansPerDay = scansPerDay
            };
            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
