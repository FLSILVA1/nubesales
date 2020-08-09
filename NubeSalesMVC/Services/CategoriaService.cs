using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NubeSalesMVC.Data;
using NubeSalesMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace NubeSalesMVC.Services
{
    public class CategoriaService
    {
        private readonly NubeSalesMVCContext _context;

        public CategoriaService(NubeSalesMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> FindAll()
        {
            return await _context.Categoria.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<List<Categoria>> FindById(int id)
        {
            return await _context.Categoria.Where(x => x.Id == id).ToListAsync();
        }

        public async Task<List<Categoria>> FindByTipo(string psTipo)
        {
            var result = from obj in _context.Categoria select obj;

            if (psTipo == "P")
            {
                result = result.Where(x => x.IntPagar == true);
            }
            else if (psTipo == "R")
            {
                result = result.Where(x => x.IntReceber == true);
            }


            return await result
                    .OrderBy(x => x.Name)
                    .ToListAsync();
        }

    }
}
