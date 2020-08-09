using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NubeSalesMVC.Models;
using Microsoft.EntityFrameworkCore;
using NubeSalesMVC.Data;
using NubeSalesMVC.Models.ViewModels;
using NubeSalesMVC.Services;

namespace NubeSalesMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly NubeSalesMVCContext _context;
        private readonly CategoriaService _categoriaService;
        public HomeController(NubeSalesMVCContext context, CategoriaService categoriaService)
        {
            _context = context;
            _categoriaService = categoriaService;
        }
        public async Task<IActionResult> Index()
        {
            var categ = await _categoriaService.FindAll();
            var viewModel = new HomeFormViewModel { Categorias = categ };
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
