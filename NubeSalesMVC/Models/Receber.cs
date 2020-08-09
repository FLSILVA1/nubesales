using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NubeSalesMVC.Models
{
    public class Receber
    {

        public int Id { get; set; }

        public Pessoa Pessoa { get; set; }

        [Display(Name = "Cliente")]
        public int PessoaId { get; set; }
        
        [Display(Name = "Vencimento")]   
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DtaMovimento { get; set; }
        
        [Display(Name = "Valor")]        
        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public double Valor { get; set; }

        [Display(Name = "Situação")]
        public int IdTipo { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        public Categoria Categoria { get; set; }

        [Display(Name = "Tipo Receita")]
        public int CategoriaId { get; set; }

        public string CarregaSituacao(int idTipo)
        {
            if (idTipo == 1)
            {
                return "Baixado";
            }
            else
            {
                return "Aberto";
            }
            
        }

    }
}
