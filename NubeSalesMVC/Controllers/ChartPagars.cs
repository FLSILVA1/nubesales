using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NubeSalesMVC.Controllers
{
    public class ChartPagars : Controller
    {
        public IActionResult Index()
        {
            return View();
        }       
        public IActionResult PieChart()
        {
            return View();
        }
    }
}
