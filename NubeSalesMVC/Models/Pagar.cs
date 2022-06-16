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
        public DateTime DataAlteracao { get; set; }
        public string UserAlteracao { get; set; }
        public int IdOrigem { get; set; }

        public Pagar()
        {
        }

        public Pagar(Pessoa pessoa, int pessoaId, DateTime dtaMovimento, double valor, int idTipo, int categoriaId, string observacao, Categoria categoria)
        {
            Pessoa = pessoa;
            PessoaId = pessoaId;
            DtaMovimento = dtaMovimento;
            Valor = valor;
            IdTipo = idTipo;
            CategoriaId = categoriaId;
            Observacao = observacao;
            Categoria = categoria;
        }

        public string CarregaSituacao(int idTipo)
        {
            if (idTipo == 1)
            {
                return "Quitado";
            }
            else
            {
                return "Aberto";
            }

        }

    }
}
