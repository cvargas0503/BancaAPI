using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.DTOs.Cuenta
{
    public class CuentaDto
    {
        public string NumeroCuenta { get; set; }
        public decimal Saldo { get; set; }
    }
}
