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
        private readonly ReceberService _receberService;
        private readonly CategoriaService _categoriaService;

        public RecebersController(NubeSalesMVCContext context, PessoaService pessoaService, ReceberService receberService, CategoriaService categoriaService)
        {
            _context = context;
            _pessoaService = pessoaService;
            _receberService = receberService;
            _categoriaService = categoriaService;
        }

        // GET: Recebers
        public async Task<IActionResult> Index(string ordem, string filtroAtual, string filtro, int? pagina, int? situacao, DateTime? minDate, DateTime? maxDate)
        {
            CarregaTipo();
            //contas vencendo - alerta
            var ctvenc = await _receberService.ContasVencendo();
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

            var contas = from conta in _context.Receber
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
            return View(await PaginatedList<Receber>.CreateAsync(contas.AsNoTracking(), pagina ?? 1, pageSize));


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
            var categ = await _categoriaService.FindById(receber.CategoriaId);
            var viewModel = new ReceberFormViewModel { Receber = receber, Pessoas = pessoas, Categorias = categ };
            return View(viewModel);
        }

        // GET: Recebers/Create
        public async Task<IActionResult> Create(int? IdCategoria)
        {
            CarregaTipo();         
            var pessoas = await _pessoaService.FindAllCliente();
            var categorias = await _categoriaService.FindByTipo("R");
            if (IdCategoria != null)
            {
                var receber = new Receber
                {
                    CategoriaId = (int)IdCategoria,
                    DtaMovimento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                };            
                var viewModel = new ReceberFormViewModel { Pessoas = pessoas, Receber = receber, Categorias = categorias };
                return View(viewModel);
            }
            else
            {
                var receber = new Receber
                {
                    DtaMovimento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                };
                var viewModel = new ReceberFormViewModel { Pessoas = pessoas, Receber = receber, Categorias = categorias };
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
            var categ = await _categoriaService.FindByTipo("R");
            var viewModel = new ReceberFormViewModel {Receber = receber, Pessoas = pessoas, Categorias = categ };
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
            var categ = await _categoriaService.FindById(receber.CategoriaId);
            var viewModel = new ReceberFormViewModel { Receber = receber, Pessoas = pessoas, Categorias = categ };
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
