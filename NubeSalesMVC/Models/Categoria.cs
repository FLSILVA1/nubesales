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
        public DateTime DataAlteracao { get; set; }
        public string UserAlteracao { get; set; }

        public ICollection<Pagar> ContasP { get; set; } = new List<Pagar>();

        public ICollection<Receber> ContasR { get; set; } = new List<Receber>();

        public double TotalPeriodoP(DateTime initial, DateTime final, int? categoria)
        {
            var result = from obj in ContasP select obj;
            if (categoria.HasValue)
            {
                result = result.Where(obj => obj.CategoriaId == categoria);
            }
            return result
                    .Where(obj => obj.DtaMovimento >= initial && obj.DtaMovimento <= final)
                    .Sum(obj => obj.Valor);
        }

        public double TotalPeriodoR(DateTime initial, DateTime final, int? categoria)
        {
            var result = from obj in ContasR select obj;
            if (categoria.HasValue)
            {
                result = result.Where(obj => obj.CategoriaId == categoria);
            }
            return result
                    .Where(obj => obj.DtaMovimento >= initial && obj.DtaMovimento <= final)
                    .Sum(obj => obj.Valor);
        }


    }
}
