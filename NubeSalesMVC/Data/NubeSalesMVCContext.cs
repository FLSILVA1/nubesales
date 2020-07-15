using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NubeSalesMVC.Models;

namespace NubeSalesMVC.Data
{
    public class NubeSalesMVCContext : DbContext
    {
        public NubeSalesMVCContext (DbContextOptions<NubeSalesMVCContext> options)
            : base(options)
        {
        }

        public DbSet<NubeSalesMVC.Models.Receber> Receber { get; set; }

        public DbSet<NubeSalesMVC.Models.Pagar> Pagar { get; set; }

        public DbSet<NubeSalesMVC.Models.Pessoa> Pessoa { get; set; }
    }
}
