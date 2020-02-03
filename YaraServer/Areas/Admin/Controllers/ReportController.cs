using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YaraServer.Data;
using YaraServer.Models;
using YaraServer.Models.ViewModels;

namespace YaraServer.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReportController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(int? Id)
        {
            if (Id != null)
            {
                var reports = await _db.Reports.Include(s => s.Terminal).Where(s => s.TerminalId == Id).ToListAsync();
                var reportsViewModel = new ReportsViewModel()
                {
                    Reports = reports
                };
                return View(reportsViewModel);
            } 
            else
            {
                var reports = await _db.Reports.Include(s => s.Terminal).ToListAsync();
                var reportsViewModel = new ReportsViewModel()
                {
                    Reports = reports
                };
                return View(reportsViewModel);
            }
        }

        // GET - Details
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var reportFromDb = await _db.Reports.Include(s => s.Terminal).FirstOrDefaultAsync(r => r.Id == Id);
            if (reportFromDb == null)
            {
                return NotFound();
            }

            var scans = await _db.Scans.Where(s => s.ReportId == reportFromDb.Id).ToListAsync();
            var messages = await _db.Messages.Where(m => m.ReportId == reportFromDb.Id).ToListAsync();
            var yaraResults = await _db.YaraResults.Where(y => y.ReportId == reportFromDb.Id).ToListAsync();

            List<YaraResultAndYaraMetasViewModel> yaraResultAndYaraMetas = new List<YaraResultAndYaraMetasViewModel>();
            foreach (var item in yaraResults)
            {
                var yaraMeta = await _db.YaraMetas.Where(y => y.YaraResultId == item.Id).ToListAsync();
                yaraResultAndYaraMetas.Add(new YaraResultAndYaraMetasViewModel()
                {
                    YaraResult = item,
                    YaraMetas = yaraMeta
                });
            }

            ReportDetailsViewModel reportDetailsViewModel = new ReportDetailsViewModel()
            {
                Report = reportFromDb,
                Scans = scans,
                YaraResults = yaraResultAndYaraMetas,
                Messages = messages
            };
            return View(reportDetailsViewModel);
        }
    }
}