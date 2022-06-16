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
using Microsoft.AspNetCore.Authorization;
using NubeSalesMVC.Extensions;

namespace NubeSalesMVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly NubeSalesMVCContext _context;
        private readonly CategoriaService _categoriaService;
        public HomeController(NubeSalesMVCContext context, CategoriaService categoriaService)
        {
            _context = context;
            _categoriaService = categoriaService;
        }
        [Route("")]
        [Route("Home")]
        [Route("Home/Index")]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string alertaModal)
        {            
            if (User.Identity.IsAuthenticated)
            {
                var categ = await _categoriaService.FindAll();
                var viewModel = new HomeFormViewModel { Categorias = categ };
                
                ViewData["AlertaModal"] = alertaModal;
                return View(viewModel);
            }
            else
            {                
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
