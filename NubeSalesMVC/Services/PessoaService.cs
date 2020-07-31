using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NubeSalesMVC.Data;
using NubeSalesMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace NubeSalesMVC.Services
{
    public class PessoaService
    {
        private readonly NubeSalesMVCContext _context;

        public PessoaService(NubeSalesMVCContext context)
        {
            _context = context;
        }

        public List<Pessoa> FindAll()
        {
            return _context.Pessoa.OrderBy(x => x.Name).ToList();
        }

        public async Task<List<Pessoa>> FindAllCredor()
        {
            return await _context.Pessoa
                    .Where(x => x.IdFinPagar)
                    .OrderBy(x => x.Name)
                    .ToListAsync();
        }
        public async Task<List<Pessoa>> FindAllCliente()
        {
            return await _context.Pessoa
                    .Where(x => x.IdFinReceber)
                    .OrderBy(x => x.Name)
                    .ToListAsync();
        }

        public async Task<List<Pessoa>> BuscaPessoa(int idPessoa)
        {
            return await _context.Pessoa
                    .Where(x => x.Id == idPessoa)
                    .ToListAsync();
        }
        public async Task<List<Pessoa>> BuscaPessoaByNameAsync(string NamePessoa)
        {
            return await _context.Pessoa
                    .Where(x => x.Name == NamePessoa)
                    .ToListAsync();
        }


        public async Task<List<Pessoa>> FindByTipoAsync(Boolean idFinPagar, Boolean idFinReceber)
        {
            var result = from obj in _context.Pessoa select obj;

            if (idFinPagar == true && idFinReceber == true)
            {
                result = result.Where(x => x.IdFinPagar == true || x.IdFinReceber == true);
            }
            else if (idFinPagar == false && idFinReceber == true)
            {
                result = result.Where(x => x.IdFinReceber == true);
            }
            else if (idFinPagar == true && idFinReceber == false)
            {
                result = result.Where(x => x.IdFinPagar == true);
            }
            else if (idFinPagar == false && idFinReceber == false)
            {
                result = result.Where(x => x.IdFinPagar == false || x.IdFinReceber == false);
            }

                return await result                    
                    .OrderByDescending(x => x.Name)
                    .ToListAsync();
        }
    }
}
