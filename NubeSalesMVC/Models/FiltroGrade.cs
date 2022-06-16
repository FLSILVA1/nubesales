using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NubeSalesMVC.Models
{
    public class FiltroGrade
    {
        public int? Situacao { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public string FiltroAtual { get; set; }
        public string Filtro { get; set; }
        public string Ordem { get; set; }
        public int? Pagina { get; set; }
    }
}
