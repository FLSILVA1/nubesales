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

        public async Task<List<Pagar>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
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

            return await result
                    .Include(x => x.Pessoa)
                    .OrderByDescending(x => x.DtaMovimento)
                    .ToListAsync();
        }
    }
}
