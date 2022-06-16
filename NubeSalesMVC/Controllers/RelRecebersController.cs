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

        public IActionResult Index(string tipoRel)
        {
            CarregaTipo_rel();
            if (tipoRel == "G")
            {
                ViewData["acaoRel"] = "GroupingSearch";
            }
            else
            {
                ViewData["acaoRel"] = "SimpleSearch";
            }
            return View();
        }
        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate, int? situacao)
        {
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
            var result = await _relReceberService.FindByDateGroupingAsync(minDate, maxDate, situacao);
            return View(result);
        }
        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate, int? situacao)
        {
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
            var result = await _relReceberService.FindByDateAsync(minDate, maxDate, situacao);
            return View(result);
        }

        public void CarregaTipo_rel()
        {
            var listaSituacao = new List<SelectListItem>
            {
                new SelectListItem{Text = "Aberto", Value = "0"},
                new SelectListItem{Text = "Quitado", Value = "1"}
            };

            ViewBag.ListaSituacao_rer = listaSituacao;

        }
    }
}
