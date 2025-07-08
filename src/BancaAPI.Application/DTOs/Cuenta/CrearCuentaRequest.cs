using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.DTOs.Cuenta
{
    public class CrearCuentaRequest
    {
        public Guid ClienteId { get; set; }
        public decimal SaldoInicial { get; set; }
    }
}
