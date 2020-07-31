using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NubeSalesMVC.Data;
using NubeSalesMVC.Models;
using NubeSalesMVC.Services;
using NubeSalesMVC.Models.ViewModels;

namespace NubeSalesMVC.Controllers
{
    public class RecebersController : Controller
    {
        private readonly NubeSalesMVCContext _context;
        private readonly PessoaService _pessoaService;

        public RecebersController(NubeSalesMVCContext context, PessoaService pessoaService)
        {
            _context = context;
            _pessoaService = pessoaService;
        }

        // GET: Recebers
        public async Task<IActionResult> Index()
        {
            var lista = await _context.Receber
                                .Include(x => x.Pessoa)
                                .ToListAsync();
            return View(lista);
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
            var pessoas = await _pessoaService.BuscaPessoa(receber.PessoaId);
            var viewModel = new ReceberFormViewModel { Receber = receber, Pessoas = pessoas };
            return View(viewModel);
        }

        // GET: Recebers/Create
        public async Task<IActionResult> Create(int? id)
        {
            CarregaTipo();         
            var pessoas = await _pessoaService.FindAllCliente();
            if (id != null)
            {
                var receber = new Receber
                {
                    IdTipoReceita = (int)id,
                    DtaMovimento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                };            
                var viewModel = new ReceberFormViewModel { Pessoas = pessoas, Receber = receber };
                return View(viewModel);
            }
            else
            {
                var receber = new Receber
                {
                    DtaMovimento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                };
                var viewModel = new ReceberFormViewModel { Pessoas = pessoas, Receber = receber };
                return View(viewModel);
            }
            
        }

        // POST: Recebers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Receber receber)
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
            CarregaTipo();
            var pessoas = await _pessoaService.BuscaPessoa(receber.PessoaId);
            var viewModel = new ReceberFormViewModel {Receber = receber, Pessoas = pessoas };
            return View(viewModel);
        }

        // POST: Recebers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Receber receber)
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
            var pessoas = await _pessoaService.BuscaPessoa(receber.PessoaId);
            var viewModel = new ReceberFormViewModel { Receber = receber, Pessoas = pessoas };
            return View(viewModel);
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

        public void CarregaTipo()
        {
            var listaSituacao = new List<SelectListItem>
            {
                new SelectListItem{Text = "Aberto", Value = "0"},
                new SelectListItem{Text = "Baixado", Value = "1"}
            };

            ViewBag.ListaSituacao = listaSituacao;

            var tipoReceita = new List<SelectListItem>
            {
                new SelectListItem{Text = "Contratos", Value = "0" },
                new SelectListItem{Text = "Serviços", Value = "1" },
                new SelectListItem{Text = "Licenças", Value = "2" },
                new SelectListItem{Text = "Hardware", Value = "3" },
                new SelectListItem{Text = "Comissões", Value = "4" }
            };

            ViewBag.ListaTipoReceita = tipoReceita;

        }

    }
}
