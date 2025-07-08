using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [MaxLength(10)]
        public string Sexo { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Ingresos { get; set; }

        // Navegación
        public ICollection<CuentaBancaria> Cuentas { get; set; } = new List<CuentaBancaria>();
    }

}
