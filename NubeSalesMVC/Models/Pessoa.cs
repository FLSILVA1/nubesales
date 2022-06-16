using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NubeSalesMVC.Models
{
    public class Pessoa
    {
        public int Id { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Credor")]
        public Boolean IdFinPagar { get; set; }

        [Display(Name = "Cliente")]
        public Boolean IdFinReceber { get; set; }
        public DateTime DataAlteracao { get; set; }
        public string UserAlteracao { get; set; }

    }
}
