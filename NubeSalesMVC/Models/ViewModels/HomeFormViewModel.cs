using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NubeSalesMVC.Models.ViewModels
{
    public class HomeFormViewModel
    {
        public ICollection<Categoria> Categorias { get; set; }
    }
}
