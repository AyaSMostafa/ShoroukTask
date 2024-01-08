﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoroukTask.Data;
using ShoroukTask.Models;

namespace ShoroukTask.Controllers
{
    public class CashiersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CashiersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cashiers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cashiers.Include(c => c.Branch).ToListAsync());
        }

        // GET: Cashiers/Details/5
        //public async Task<IActionResult> Index()
        //{
        //    var cashiers = await _context.Cashiers.Include(c => c.Branch).ToListAsync();
        //    var cashierViewModels = cashiers.Select(c => new CashierViewModel
        //    {
        //        CashierName = c.CashierName,
        //        BranchName = c.Branch?.BranchName ?? "N/A" // If no branch is associated, display "N/A"
        //    }).ToList();

        //    return View(cashierViewModels);
        //}



        // GET: Cashiers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cashiers == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cashier == null)
            {
                return NotFound();
            }

            return View(cashier);
        }

        // GET: Cashiers/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "BranchName");
            return View();
        }

        // POST: Cashiers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CashierName,BranchId")] Cashier cashier)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cashier);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "BranchName", cashier.BranchId);
            return View(cashier);
        }

        // GET: Cashiers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cashiers == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "BranchName", cashier.BranchId);
            return View(cashier);
        }

        // POST: Cashiers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CashierName,BranchId")] Cashier cashier)
        {
            if (id != cashier.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cashier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CashierExists(cashier.Id))
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "BranchName", cashier.BranchId);
            return View(cashier);
        }

        // GET: Cashiers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cashiers == null)
            {
                return NotFound();
            }

            var cashier = await _context.Cashiers
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cashier == null)
            {
                return NotFound();
            }

            return View(cashier);
        }

        // POST: Cashiers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cashiers == null)
            {
                return Problem("Entity set 'ShaTaskContext.Cashiers'  is null.");
            }
            var cashier = await _context.Cashiers.FindAsync(id);
            if (cashier != null)
            {
                _context.Cashiers.Remove(cashier);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CashierExists(int id)
        {
          return (_context.Cashiers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
