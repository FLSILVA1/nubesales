using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace NubeSalesMVC.Models.ViewModels
{
    public class ReceberFormViewModel
    {
        public Receber Receber { get; set; }

        public ICollection<Pessoa> Pessoas { get; set; }

        public ICollection<Categoria> Categorias { get; set; }

        [Range(1, 120)]
        public int? NroParcelas { get; set; }
        
        public FiltroGrade FiltroIndex { get; set; }
        public List<SituacaoDoc> SituacaoDoc { get; set; }
        public Boolean AlterarTodos { get; set; }

    }
}
