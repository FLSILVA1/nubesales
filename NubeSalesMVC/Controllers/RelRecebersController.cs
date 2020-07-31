using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NubeSalesMVC.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NubeSalesMVC.Controllers
{
    public class RelRecebersController : Controller
    {

        private readonly RelReceberService _relReceberService;

        public RelRecebersController(RelReceberService relReceberService)
        {
            _relReceberService = relReceberService;
        }

        public IActionResult Index()
        {
            CarregaTipo_rel();
            return View();
        }
        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now;
            }
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var result = await _relReceberService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        public void CarregaTipo_rel()
        {
            var listaSituacao = new List<SelectListItem>
            {
                new SelectListItem{Text = "Aberto", Value = "0"},
                new SelectListItem{Text = "Baixado", Value = "1"}
            };

            ViewBag.ListaSituacao_rer = listaSituacao;

            var tipoReceita = new List<SelectListItem>
            {
                new SelectListItem{Text = "Contratos", Value = "0" },
                new SelectListItem{Text = "Serviços", Value = "1" },
                new SelectListItem{Text = "Licenças", Value = "2" },
                new SelectListItem{Text = "Hardware", Value = "3" }
            };

            ViewBag.ListaTipoReceita_rel = tipoReceita;

        }
    }
}
