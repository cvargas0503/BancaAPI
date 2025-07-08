using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Domain.Entities
{
    public class CuentaBancaria
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumeroCuenta { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Saldo { get; set; }

        // FK y navegación
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }

}
