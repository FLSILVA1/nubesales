using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NubeSalesMVC.Services;

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
            var result = await _relPagarService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }
    }
}
