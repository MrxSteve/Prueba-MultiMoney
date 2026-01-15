using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnicaMultiMoney.Models
{
    public class Cuota
    {
        public int Id { get; set; }

        public int PrestamoId { get; set; }

        public int NumeroCuota { get; set; }

        [Column(TypeName = "decimal(18,7)")]
        public decimal CuotaFija { get; set; }

        [Column(TypeName = "decimal(18,7)")]
        public decimal Capital { get; set; }

        [Column(TypeName = "decimal(18,7)")]
        public decimal Interes { get; set; }

        [Column(TypeName = "decimal(18,7)")]
        public decimal SaldoPendiente { get; set; }

        [ForeignKey("PrestamoId")]
        public Prestamo Prestamo { get; set; } = null!;
    }
}
