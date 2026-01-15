using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnicaMultiMoney.Models
{
    public class Prestamo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        [Range(1000, 40000, ErrorMessage = "El monto debe estar entre $1,000 y $40,000")]
        [Column(TypeName = "decimal(18,7)")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El plazo es requerido")]
        [Range(12, int.MaxValue, ErrorMessage = "El plazo mínimo es de 12 meses")]
        public int PlazoMeses { get; set; }

        [Column(TypeName = "decimal(18,7)")]
        public decimal TasaMensual { get; set; } = 1.75m;

        [Column(TypeName = "decimal(18,7)")]
        public decimal Comision { get; set; }

        [Column(TypeName = "decimal(18,7)")]
        public decimal CuotaMensual { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public List<Cuota> Cuotas { get; set; } = new List<Cuota>();
    }
}