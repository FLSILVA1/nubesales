using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NubeSalesMVC.Models.ViewModels
{
    public class PagarFormViewModel
    {
        public Pagar Pagar { get; set; }

        public ICollection<Pessoa> Pessoas { get; set; }
        public ICollection<Categoria> Categorias { get; set; }

        [Range(1,120)]
        public int? NroParcelas { get; set; }

        public IEnumerable<ImagemViewModel> Imagens { get; set; }


    }
}
