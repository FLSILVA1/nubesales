using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NubeSalesMVC.Models.ViewModels
{
    public class PagarFormViewModel
    {
        public Pagar Pagar { get; set; }

        public ICollection<Pessoa> Pessoas { get; set; }
    }
}
