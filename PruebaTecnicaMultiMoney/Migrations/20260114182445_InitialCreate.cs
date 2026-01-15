using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PruebaTecnicaMultiMoney.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Monto = table.Column<decimal>(type: "decimal(18,7)", nullable: false),
                    PlazoMeses = table.Column<int>(type: "int", nullable: false),
                    TasaMensual = table.Column<decimal>(type: "decimal(18,7)", nullable: false),
                    Comision = table.Column<decimal>(type: "decimal(18,7)", nullable: false),
                    CuotaMensual = table.Column<decimal>(type: "decimal(18,7)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cuotas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrestamoId = table.Column<int>(type: "int", nullable: false),
                    NumeroCuota = table.Column<int>(type: "int", nullable: false),
                    CuotaFija = table.Column<decimal>(type: "decimal(18,7)", nullable: false),
                    Capital = table.Column<decimal>(type: "decimal(18,7)", nullable: false),
                    Interes = table.Column<decimal>(type: "decimal(18,7)", nullable: false),
                    SaldoPendiente = table.Column<decimal>(type: "decimal(18,7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuotas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cuotas_Prestamos_PrestamoId",
                        column: x => x.PrestamoId,
                        principalTable: "Prestamos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cuotas_PrestamoId",
                table: "Cuotas",
                column: "PrestamoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cuotas");

            migrationBuilder.DropTable(
                name: "Prestamos");
        }
    }
}
