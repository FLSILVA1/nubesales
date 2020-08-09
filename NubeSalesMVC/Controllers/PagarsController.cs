using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NubeSalesMVC.Data;
using NubeSalesMVC.Models;
using NubeSalesMVC.Models.ViewModels;
using NubeSalesMVC.Services;

namespace NubeSalesMVC.Controllers
{
    public class PagarsController : Controller
    {
        private readonly NubeSalesMVCContext _context;
        private readonly PessoaService _pessoaService;
        private readonly PagarService _pagarService;
        private readonly CategoriaService _categoriaService;

        public PagarsController(NubeSalesMVCContext context, PessoaService pessoaService, PagarService pagarService, CategoriaService categoriaService)
        {
            _context = context;
            _pessoaService = pessoaService;
            _pagarService = pagarService;
            _categoriaService = categoriaService;
        }

        // GET: Pagars
        public async Task<IActionResult> Index(string ordem, string filtroAtual, string filtro, int? pagina, int? situacao, DateTime? minDate, DateTime? maxDate)
        {
            CarregaTipo();
            //contas vencendo - alerta
            var ctvenc = await _pagarService.ContasVencendo();
            var qtde = ctvenc.Count;
            if (qtde > 0)
            {
                ViewData["MsgALerta"] = "Atenção! Existem " + qtde.ToString() + " contas próximas ao vencimento ou vencidas.";
            }
            ViewData["ordemAtual"] = ordem;
            ViewData["NomeParm"] = String.IsNullOrEmpty(ordem) ? "nome_desc" : "";
            ViewData["situacao"] = situacao.GetValueOrDefault(0);
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            if (filtro != null)
            {
                pagina = 1;
            }
            else
            {
                filtro = filtroAtual;
            }

            ViewData["filtroAtual"] = filtro;

            var contas = from conta in _context.Pagar
                         select conta;


            if (!String.IsNullOrEmpty(filtro))
            {
                contas = contas.Where(conta => conta.Pessoa.Name.Contains(filtro));
            }
            //filtro de data e situacao
            contas = contas
                        .Where(conta => conta.DtaMovimento >= minDate.Value & conta.DtaMovimento <= maxDate.Value)
                        .Where(conta => conta.IdTipo == situacao.GetValueOrDefault(0));

            switch (ordem)
            {
                case "nome_desc":
                    contas = contas.OrderByDescending(conta => conta.DtaMovimento);
                    break;
                default:
                    contas = contas.OrderBy(conta => conta.DtaMovimento);
                    break;
            }


            contas = contas.Include(x => x.Pessoa);
            contas = contas.Include(x => x.Categoria);

            int pageSize = 15;
            return View(await PaginatedList<Pagar>.CreateAsync(contas.AsNoTracking(), pagina ?? 1, pageSize));


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

            var pessoas = await _pessoaService.BuscaPessoa(pagar.PessoaId);
            var categ = await _categoriaService.FindById(pagar.CategoriaId);
            var viewModel = new PagarFormViewModel { Pagar = pagar, Pessoas = pessoas };
            return View(viewModel);
        }

        // GET: Pagars/Create
        public async Task<IActionResult> Create(int? id)
        {
            CarregaTipo();
            var pessoas = await _pessoaService.FindAllCredor();
            var categorias = await _categoriaService.FindByTipo("P");
            if (id != null)
            {
                var pagar = new Pagar
                {
                    CategoriaId = (int)id,
                    DtaMovimento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                };
                var viewModel = new PagarFormViewModel { Pessoas = pessoas, Pagar = pagar, Categorias = categorias };
                return View(viewModel);
            }
            else
            {
                var pagar = new Pagar
                {
                    DtaMovimento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                };
                var viewModel = new PagarFormViewModel { Pessoas = pessoas, Pagar = pagar, Categorias = categorias };
                return View(viewModel);
            }
        }

        // POST: Pagars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pagar pagar)
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
            CarregaTipo();
            var pessoas = await _pessoaService.BuscaPessoa(pagar.PessoaId);
            var categ = await _categoriaService.FindByTipo("P");
            var viewModel = new PagarFormViewModel { Pagar = pagar, Pessoas = pessoas, Categorias = categ };
            return View(viewModel);
        }

        // POST: Pagars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pagar pagar)
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

            var pessoas = await _pessoaService.BuscaPessoa(pagar.PessoaId);
            var categ = await _categoriaService.FindById(pagar.CategoriaId);
            var viewModel = new PagarFormViewModel { Pagar = pagar, Pessoas = pessoas, Categorias = categ };
            return View(viewModel);
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

        public void CarregaTipo()
        {
            var listaSituacao = new List<SelectListItem>
            {
                new SelectListItem{Text = "Aberto", Value = "0"},
                new SelectListItem{Text = "Baixado", Value = "1"}
            };

            ViewBag.ListaSituacao = listaSituacao;

            var tipoDespesa = new List<SelectListItem>
            {
                new SelectListItem{Text = "Despesas fixas", Value = "0" },
                new SelectListItem{Text = "Despesas variáveis", Value = "1" },
            };

            ViewBag.ListaTipoDespesa = tipoDespesa;
        }

    }
}
