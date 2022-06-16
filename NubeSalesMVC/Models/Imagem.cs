using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NubeSalesMVC.Models
{
    public class Imagem
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string ContentType { get; set; }
        public byte[] Dados { get; set; }

    }
}
