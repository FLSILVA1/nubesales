using NubeSalesMVC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NubeSalesMVC.Services
{
    public class ReceberValidacaoService
    {
        public string ReceberValido(ReceberFormViewModel contaAReceber)
        {
            var msgErro = "";
            if (contaAReceber.NroParcelas > 120)
            {
                msgErro = "O máximo são 120 parcelas.";
            }
            return msgErro;
        }
}
}
