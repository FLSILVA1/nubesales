using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NubeSalesMVC.Models;
using NubeSalesMVC.Data;
using System.Globalization;

namespace NubeSalesMVC.Services
{
    public class ReceberService
    {
        private readonly NubeSalesMVCContext _context;

        public ReceberService(NubeSalesMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Receber>> ContasVencendo()
        {
            return await _context.Receber
                    .Where(x => x.DtaMovimento <= DateTime.Now)
                    .Where(x => x.IdTipo == 0)
                    .ToListAsync();
        }

        public async Task<List<Receber>> BuscarContasVinculadas(Receber contaReceber)
        {
            if (contaReceber.IdOrigem == 0)
            {
                return await _context.Receber
                        .Where(x => x.IdOrigem == contaReceber.Id)
                        .Where(x => x.IdTipo == 0)
                        .ToListAsync();
            } else
            {
                return await _context.Receber
                        .Where(x => x.IdOrigem == contaReceber.IdOrigem || x.Id == contaReceber.IdOrigem)
                        .Where(x => x.Id != contaReceber.Id)
                        .Where(x => x.IdTipo == 0)
                        .ToListAsync();
            }
        }
        public async Task<List<Receber>> BuscarContasVinculadasById(int Id)
        {
            var contaReceber = await _context.Receber.Where(x => x.Id == Id).MaxAsync();

            if (contaReceber.IdOrigem == 0)
            {
                return await _context.Receber
                        .Where(x => x.IdOrigem == contaReceber.Id)
                        .Where(x => x.IdTipo == 0)
                        .ToListAsync();
            }
            else
            {
                return await _context.Receber
                        .Where(x => x.IdOrigem == contaReceber.IdOrigem || x.Id == contaReceber.IdOrigem)
                        .Where(x => x.Id != contaReceber.Id)
                        .Where(x => x.IdTipo == 0)
                        .ToListAsync();
            }
        }
    }
}
