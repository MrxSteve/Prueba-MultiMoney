using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaMultiMoney.Data;
using PruebaTecnicaMultiMoney.Models;

namespace PruebaTecnicaMultiMoney.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const decimal TASA_MENSUAL = 1.75m;
        private const decimal PORCENTAJE_COMISION = 0.05m;

        public PrestamosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prestamos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prestamos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    prestamo.Comision = Math.Round(prestamo.Monto * PORCENTAJE_COMISION, 7);

                    prestamo.TasaMensual = TASA_MENSUAL;

                    prestamo.CuotaMensual = CalcularCuotaFija(prestamo.Monto, prestamo.PlazoMeses, TASA_MENSUAL);

                    prestamo.FechaCreacion = DateTime.Now;

                    prestamo.Cuotas = GenerarCronogramaCuotas(prestamo.Monto, prestamo.CuotaMensual, prestamo.PlazoMeses, TASA_MENSUAL);

                    _context.Prestamos.Add(prestamo);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return RedirectToAction(nameof(Details), new { id = prestamo.Id });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    ModelState.AddModelError("", $"Error al guardar el prestamo: {ex.Message}");
                }
            }

            return View(prestamo);
        }

        // GET: Prestamos/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .Include(p => p.Cuotas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        /// Calcular la cuota fija mensual usando la formula de amortizacion francesa
        private decimal CalcularCuotaFija(decimal monto, int plazoMeses, decimal tasaMensualPorcentaje)
        {
            decimal tasaDecimal = tasaMensualPorcentaje / 100m;

            // Formula: CuotaFija = Monto × [i × (1 + i)^n] / [(1 + i)^n - 1]
            // Donde: i = tasa mensual, n = # de meses

            decimal unMasTasa = 1m + tasaDecimal;
            decimal potencia = (decimal)Math.Pow((double)unMasTasa, plazoMeses);

            decimal numerador = tasaDecimal * potencia;
            decimal denominador = potencia - 1m;

            decimal cuotaFija = monto * (numerador / denominador);

            // Redondear a 7 decimales
            return Math.Round(cuotaFija, 7);
        }

        /// Genera el cronograma completo de cuotas (una fila por mes)
        private List<Cuota> GenerarCronogramaCuotas(decimal monto, decimal cuotaFija, int plazoMeses, decimal tasaMensualPorcentaje)
        {
            var cuotas = new List<Cuota>();
            decimal tasaDecimal = tasaMensualPorcentaje / 100m;
            decimal saldoPendiente = monto;

            for (int i = 1; i <= plazoMeses; i++)
            {
                decimal interes = Math.Round(saldoPendiente * tasaDecimal, 7);

                decimal capital = Math.Round(cuotaFija - interes, 7);

                saldoPendiente = Math.Round(saldoPendiente - capital, 7);

                if (i == plazoMeses && saldoPendiente != 0)
                {
                    capital += saldoPendiente;
                    saldoPendiente = 0;
                }

                var cuota = new Cuota
                {
                    NumeroCuota = i,
                    CuotaFija = cuotaFija,
                    Capital = capital,
                    Interes = interes,
                    SaldoPendiente = saldoPendiente
                };

                cuotas.Add(cuota);
            }

            return cuotas;
        }

        // GET: Prestamos/Index
        public async Task<IActionResult> Index()
        {
            var prestamos = await _context.Prestamos
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();

            return View(prestamos);
        }
    }
}
