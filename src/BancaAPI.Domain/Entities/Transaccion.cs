using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Domain.Entities
{
    public class Transaccion
    {
        public Guid Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoDespues { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Relaciones
        public Guid CuentaId { get; set; }
        public CuentaBancaria Cuenta { get; set; }

        public Guid TipoTransaccionId { get; set; }
        public TipoTransaccion TipoTransaccion { get; set; }
    }

}
