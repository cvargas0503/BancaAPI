using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.DTOs.Transaccion
{
    public class TransaccionDto
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; } // "Depósito" o "Retiro"
        public decimal Monto { get; set; }
        public decimal SaldoDespues { get; set; }
        public DateTime Fecha { get; set; }
    }
}
