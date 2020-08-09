using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NubeSalesMVC.Services;
using NubeSalesMVC.Data;
using NubeSalesMVC.Services.Exceptions;

namespace NubeSalesMVC.Models
{
    public class PessoasController : Controller
    {
        private readonly NubeSalesMVCContext _context;
        private readonly PessoaService _pessoaService;

        public PessoasController(NubeSalesMVCContext context, PessoaService pessoaService)
        {
            _context = context;
            _pessoaService = pessoaService;
        }


        // GET: Pessoas
        public async Task<IActionResult> Index(string ordem, string filtroAtual, string filtro, int? pagina)
        {

            ViewData["ordemAtual"] = ordem;
            ViewData["NomeParm"] = String.IsNullOrEmpty(ordem) ? "nome_desc" : "";
            if (filtro != null)
            {
                pagina = 1;
            }
            else
            {
                filtro = filtroAtual;
            }

            ViewData["filtroAtual"] = filtro;

            var estudantes = from est in _context.Pessoa
                             select est;


            if (!String.IsNullOrEmpty(filtro))
            {
                estudantes = estudantes.Where(est => est.Name.Contains(filtro));
            }

            switch (ordem)
            {
                case "nome_desc":
                    estudantes = estudantes.OrderByDescending(est => est.Name);
                    break;
                default:
                    estudantes = estudantes.OrderBy(est => est.Name);
                    break;
            }

            int pageSize = 15;
            return View(await PaginatedList<Pessoa>.CreateAsync(estudantes.AsNoTracking(), pagina ?? 1, pageSize));

            //return View(await _context.Pessoa.ToListAsync());            

        }

        // GET: Pessoas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // GET: Pessoas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pessoas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pessoa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pessoa);
        }

        // GET: Pessoas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            return View(pessoa);
        }

        // POST: Pessoas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pessoa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PessoaExists(pessoa.Id))
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
            return View(pessoa);
        }

        // GET: Pessoas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pessoa = await _context.Pessoa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pessoa == null)
            {
                return NotFound();
            }

            return View(pessoa);
        }

        // POST: Pessoas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var pessoa = await _context.Pessoa.FindAsync(id);
            _context.Pessoa.Remove(pessoa);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        private bool PessoaExists(int id)
        {
            return _context.Pessoa.Any(e => e.Id == id);
        }

        public async Task<IActionResult> SimpleSearch(Boolean idFinPagar, Boolean idFinReceber)
        {

            var result = await _pessoaService.FindByTipoAsync(idFinPagar, idFinReceber);
            return View(result);
        }
    }
}
