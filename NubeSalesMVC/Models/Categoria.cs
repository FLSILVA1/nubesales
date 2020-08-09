using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NubeSalesMVC.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Display(Name = "Categoria")]
        public string Name { get; set; }

        [Display(Name = "Despesa")]
        public Boolean IntPagar { get; set; }

        [Display(Name = "Receita")]
        public Boolean IntReceber { get; set; }

    }
}
