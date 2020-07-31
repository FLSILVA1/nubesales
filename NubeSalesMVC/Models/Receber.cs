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
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DtaMovimento { get; set; }
        
        [Display(Name = "Valor")]        
        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        public double Valor { get; set; }

        [Display(Name = "Situação")]
        public int IdTipo { get; set; }

        [Display(Name = "Tipo Receita")]
        public int IdTipoReceita { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

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
        public string CarregaTipoReceita(int idTipoReceita)
        {
            if (idTipoReceita == 0)
            {
                return "Contratos";
            }
            else if (idTipoReceita == 1)
            {
                return "Serviços";
            }
            else if (idTipoReceita == 2)
            {
                return "Licenças";
            }
            else if (idTipoReceita == 3)
            {
                return "Hardware";
            }
            else
            {
                return "<Não definido>";
            }
        }

    }
}
