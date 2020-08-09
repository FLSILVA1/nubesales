using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace NubeSalesMVC.Models
{
    public class Pagar
    {
        public int Id { get; set; }

        public Pessoa Pessoa { get; set; }

        [Display(Name = "Credor")]
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

        [Display(Name="Tipo Despesa")]
        public int CategoriaId { get; set; }

        [Display(Name="Observação")]
        public string Observacao { get; set; }

        public Categoria Categoria { get; set; }



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

        public string CarregaTipoDespesa(int idTipoDespesa)
        {
            if (idTipoDespesa == 0)
            {
                return "Despesas fixas";
            }
            else if (idTipoDespesa == 1)
            {
                return "Despesas variáveis";
            }
            else
            {
                return "<Não definido>";
            }
        }
    }
}
