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
    public class PagarsController : Controller
    {
        private readonly NubeSalesMVCContext _context;

        public PagarsController(NubeSalesMVCContext context)
        {
            _context = context;
        }

        // GET: Pagars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pagar.ToListAsync());
        }

        // GET: Pagars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pagar = await _context.Pagar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pagar == null)
            {
                return NotFound();
            }

            return View(pagar);
        }

        // GET: Pagars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pagars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdPessoa,DtaMovimento,Valor,IdTipo")] Pagar pagar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pagar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pagar);
        }

        // GET: Pagars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pagar = await _context.Pagar.FindAsync(id);
            if (pagar == null)
            {
                return NotFound();
            }
            return View(pagar);
        }

        // POST: Pagars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdPessoa,DtaMovimento,Valor,IdTipo")] Pagar pagar)
        {
            if (id != pagar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pagar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagarExists(pagar.Id))
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
            return View(pagar);
        }

        // GET: Pagars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pagar = await _context.Pagar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pagar == null)
            {
                return NotFound();
            }

            return View(pagar);
        }

        // POST: Pagars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pagar = await _context.Pagar.FindAsync(id);
            _context.Pagar.Remove(pagar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagarExists(int id)
        {
            return _context.Pagar.Any(e => e.Id == id);
        }
    }
}
