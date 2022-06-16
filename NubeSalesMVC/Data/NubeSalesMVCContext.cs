using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NubeSalesMVC.Models;

namespace NubeSalesMVC.Data
{
    public class NubeSalesMVCContext : IdentityDbContext
    {
        public NubeSalesMVCContext (DbContextOptions<NubeSalesMVCContext> options)
            : base(options)
        {
        }

        public DbSet<Receber> Receber { get; set; }

        public DbSet<Pagar> Pagar { get; set; }

        public DbSet<Pessoa> Pessoa { get; set; }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Imagem> Imagens { get; set; }
    }
}
