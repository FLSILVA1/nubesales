using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NubeSalesMVC.Models
{
    public class Pagar
    {
        public int Id { get; set; }

        public int IdPessoa { get; set; }

        public DateTime DtaMovimento { get; set; }

        public double Valor { get; set; }

        public int IdTipo { get; set; }
    }
}
