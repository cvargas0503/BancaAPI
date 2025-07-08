using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.DTOs.Transaccion
{
    public class HistorialTransaccionesDto
    {
        public string NumeroCuenta { get; set; }
        public decimal SaldoFinal { get; set; }
        public List<TransaccionListaDto> Transacciones { get; set; }
    }
}
