using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTSolution.Migrations
{
    /// <inheritdoc />
    public partial class eleventh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoTemperatura",
                table: "Leituras");

                migrationBuilder.DropColumn(
                name: "Temperatura",
                table: "Leituras");

            migrationBuilder.AddColumn<decimal>(
                name: "LimiteInferiorTemperatura",
                table: "Sensors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LimiteSuperiorTemperatura",
                table: "Sensors",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
           name: "Temperatura",
           table: "Leituras",
           type: "decimal(18,2)",
           nullable: false,
           defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    IdAlerta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDispositivo = table.Column<int>(type: "int", nullable: false),
                    IdSensor = table.Column<int>(type: "int", nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MensagemEnviada = table.Column<bool>(type: "bit", nullable: false),
                    DataHoraAlerta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.IdAlerta);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropColumn(
                name: "LimiteInferiorTemperatura",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "LimiteSuperiorTemperatura",
                table: "Sensors");

            migrationBuilder.AlterColumn<string>(
                name: "Temperatura",
                table: "Leituras",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "TipoTemperatura",
                table: "Leituras",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
