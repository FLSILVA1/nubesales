using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NubeSalesMVC.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NubeSalesMVC.Controllers
{
    public class RelPagarsController : Controller
    {

        private readonly RelPagarService _relPagarService;

        public RelPagarsController(RelPagarService relPagarService)
        {
            _relPagarService = relPagarService;
        }

        public IActionResult Index()
        {
            CarregaTipo_rel();
            return View();
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
            var result = await _relPagarService.FindByDateAsync(minDate, maxDate, situacao);
            return View(result);
        }
        public void CarregaTipo_rel()
        {
            var listaSituacao = new List<SelectListItem>
            {
                new SelectListItem{Text = "Aberto", Value = "0"},
                new SelectListItem{Text = "Baixado", Value = "1"}
            };

            ViewBag.ListaSituacao_rec = listaSituacao;
        }
    }
}
