using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NubeSalesMVC.Data;
using NubeSalesMVC.Models;

namespace NubeSalesMVC.Controllers
{
    public class RecebersController : Controller
    {
        private readonly NubeSalesMVCContext _context;

        public RecebersController(NubeSalesMVCContext context)
        {
            _context = context;
        }

        // GET: Recebers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Receber.ToListAsync());
        }

        // GET: Recebers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receber = await _context.Receber
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receber == null)
            {
                return NotFound();
            }

            return View(receber);
        }

        // GET: Recebers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recebers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPessoa,DtaMovimento,Valor,IdTipo")] Receber receber)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(receber);
        }

        // GET: Recebers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receber = await _context.Receber.FindAsync(id);
            if (receber == null)
            {
                return NotFound();
            }
            return View(receber);
        }

        // POST: Recebers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdPessoa,DtaMovimento,Valor,IdTipo")] Receber receber)
        {
            if (id != receber.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receber);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceberExists(receber.Id))
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
            return View(receber);
        }

        // GET: Recebers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receber = await _context.Receber
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receber == null)
            {
                return NotFound();
            }

            return View(receber);
        }

        // POST: Recebers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receber = await _context.Receber.FindAsync(id);
            _context.Receber.Remove(receber);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceberExists(int id)
        {
            return _context.Receber.Any(e => e.Id == id);
        }
    }
}
