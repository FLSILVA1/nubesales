using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace NubeSalesMVC.Models
{
    public class Login
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Informe o usuário")]
        public string Usuario { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Informe a senha")]
        public string Senha { get; set; }
    }
}
