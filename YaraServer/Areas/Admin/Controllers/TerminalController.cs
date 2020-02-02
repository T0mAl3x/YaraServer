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
    public class TerminalController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TerminalController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var terminals = await _db.Terminals.Include(s => s.Certificate).ToListAsync();
            var terminalsViewModel = new TerminalsViewModel()
            {
                Terminals = terminals
            };
            return View(terminalsViewModel);
        }

        // GET - DETAILS
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            TerminalDetailsModel terminalDetailsModel = await _db.Terminals.Include(t => t.Certificate).SingleOrDefaultAsync(m => m.Id == Id);

            if (terminalDetailsModel == null)
            {
                return NotFound();
            }

            return PartialView("_IndividualTerminalDetails", terminalDetailsModel);
        }
    }
}