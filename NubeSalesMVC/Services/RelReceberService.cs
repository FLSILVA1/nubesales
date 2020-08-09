using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NubeSalesMVC.Data;
using NubeSalesMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace NubeSalesMVC.Services
{
    public class RelReceberService
    {
        private readonly NubeSalesMVCContext _context;

        public RelReceberService(NubeSalesMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Receber>> FindByDateAsync(DateTime? minDate, DateTime? maxDate, int? situacao)
        {
            var result = from obj in _context.Receber select obj;
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
    }
}
