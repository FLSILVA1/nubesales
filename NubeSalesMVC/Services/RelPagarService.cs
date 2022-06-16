using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NubeSalesMVC.Data;
using NubeSalesMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace NubeSalesMVC.Services
{
    public class RelPagarService
    {
        private readonly NubeSalesMVCContext _context;

        public RelPagarService(NubeSalesMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Pagar>> FindByDateAsync(DateTime? minDate, DateTime? maxDate, int? situacao)
        {
            var result = from obj in _context.Pagar select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.DtaMovimento >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.DtaMovimento <= maxDate.Value);
            }
            if (situacao.HasValue)
            {
                result = result.Where(x => x.IdTipo == situacao.Value);
            }
            return await result
                    .Include(x => x.Pessoa)
                    .Include(x => x.Categoria)
                    .OrderBy(x => x.DtaMovimento)
                    .ToListAsync();
        }
        public async Task<List<IGrouping<Categoria, Pagar>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate, int? situacao)
        {
            var result = from obj in _context.Pagar select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.DtaMovimento >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.DtaMovimento <= maxDate.Value);
            }
            if (situacao.HasValue)
            {
                result = result.Where(x => x.IdTipo == situacao.Value);
            }
            var listaPagar = await result
                    .Include(x => x.Pessoa)
                    .Include(x => x.Categoria)
                    .OrderBy(x => x.DtaMovimento)                    
                    .ToListAsync();

            var agrupamento = listaPagar.GroupBy(x => x.Categoria).ToList();

            return agrupamento;
        }
    }
}
