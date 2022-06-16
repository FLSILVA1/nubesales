using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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


        public async Task<IActionResult> FilteredIndex()
        {

            FiltroGrade filtroGradePagar = JsonSerializer.Deserialize<FiltroGrade>(TempData["FiltroGrade"] as string);

            if (filtroGradePagar != null)
            {
                return RedirectToAction("Index",
                    new
                    {
                        ordem = filtroGradePagar.Ordem,
                        filtroAtual = filtroGradePagar.FiltroAtual,
                        filtro = filtroGradePagar.Filtro,
                        pagina = filtroGradePagar.Pagina,
                        situacao = filtroGradePagar.Situacao,
                        minDate = filtroGradePagar.MinDate,
                        maxDate = filtroGradePagar.MaxDate
                    });
            }

            return RedirectToAction("Index");
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

            //Guarda o filtro atual
            var filtroGrade = new FiltroGrade()
            {
                Ordem = ordem,
                Filtro = filtro,
                FiltroAtual = filtroAtual,
                MinDate = minDate,
                MaxDate = maxDate,
                Pagina = pagina,
                Situacao = situacao
            };
            TempData["FiltroGrade"] = JsonSerializer.Serialize(filtroGrade);

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
                var viewModel = new PagarFormViewModel { Pessoas = pessoas, Pagar = pagar, Categorias = categorias, NroParcelas = 1 };
                return View(viewModel);
            }
            else
            {
                var pagar = new Pagar
                {
                    DtaMovimento = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)
                };
                var viewModel = new PagarFormViewModel { Pessoas = pessoas, Pagar = pagar, Categorias = categorias, NroParcelas = 1 };
                return View(viewModel);
            }
        }

        // POST: Pagars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pagar pagar, int? nroParcelas)
        {
            if (ModelState.IsValid)
            {
                if (!nroParcelas.HasValue || nroParcelas < 0) { nroParcelas = 1; }
                if (nroParcelas.HasValue && nroParcelas > 1)
                {
                    List<Pagar> parcelas = new List<Pagar> { };
                    for (int i = 1; i <= nroParcelas; i++)
                    {
                        parcelas.Add(item: new Pagar(
                            pagar.Pessoa,
                            pagar.PessoaId,
                            pagar.DtaMovimento.AddMonths(i - 1),
                            pagar.Valor,
                            pagar.IdTipo,
                            pagar.CategoriaId,
                            pagar.Observacao + " (Parc. " + i + "/" + nroParcelas + ")",
                            pagar.Categoria));
                    }

                    var idOrigem = 0;
                    foreach (var poPagar in parcelas)
                    {
                        poPagar.IdOrigem = idOrigem;

                        _context.Add(poPagar);
                        await _context.SaveChangesAsync();

                        if (idOrigem == 0)
                        {
                            idOrigem = poPagar.IdOrigem;
                        }
                    }
                }
                else
                {
                    _context.Add(pagar);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(FilteredIndex));

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
                return RedirectToAction(nameof(FilteredIndex));
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
            return RedirectToAction(nameof(FilteredIndex));
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
                new SelectListItem{Text = "Quitado", Value = "1"}
            };

            ViewBag.ListaSituacao = listaSituacao;

        }

        
        [HttpPost]
        public async Task<IActionResult> UploadImagem(IList<IFormFile> arquivos)
        {
            IFormFile imagemEnviada = arquivos.FirstOrDefault();
            if (imagemEnviada != null || imagemEnviada.ContentType.ToLower().StartsWith("image/"))
            {
                MemoryStream ms = new MemoryStream();
                imagemEnviada.OpenReadStream().CopyTo(ms);

                Imagem imagemEntity = new Imagem()
                {
                    Id = NextImagemId(),
                    Nome = imagemEnviada.FileName,
                    Dados = ms.ToArray(),
                    ContentType = imagemEnviada.ContentType
                };
                _context.Imagens.Add(imagemEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        private int NextImagemId()
        {
            var proximoID = _context.Imagens.Max(x => x.Id);
            proximoID++;

            return proximoID;
        }

        [HttpGet]
        public FileStreamResult VerImagem(int id)
        {
            Imagem imagem = _context.Imagens.FirstOrDefault(m => m.Id == id);
            MemoryStream ms = new MemoryStream(imagem.Dados);
            return new FileStreamResult(ms, imagem.ContentType);
        }

        [HttpPost, ActionName("DeleteImg")]
        public async Task<IActionResult> DelImagem(int id)
        {
            var img = await _context.Imagens.FindAsync(id);
            try
            {
                _context.Imagens.Remove(img);
                await _context.SaveChangesAsync();
                ViewData["msgErro"] = "";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewData["msgErro"] = "Erro! Não foi possível excluir a informação";
                return RedirectToAction(nameof(Index));
            }

        }

      
    }
}
