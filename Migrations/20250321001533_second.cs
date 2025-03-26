using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTSolution.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Strings");

            migrationBuilder.CreateTable(
                name: "Dispositivos",
                columns: table => new
                {
                    IdDispositivo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHoraCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispositivos", x => x.IdDispositivo);
                });

            migrationBuilder.CreateTable(
                name: "Leituras",
                columns: table => new
                {
                    IdLeitura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdDispositivo = table.Column<int>(type: "int", nullable: false),
                    IdSensor = table.Column<int>(type: "int", nullable: false),
                    Temperatura = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataHoraLeitura = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leituras", x => x.IdLeitura);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    IdSensor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataHoraCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DispositivosIdDispositivo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.IdSensor);
                    table.ForeignKey(
                        name: "FK_Sensors_Dispositivos_DispositivosIdDispositivo",
                        column: x => x.DispositivosIdDispositivo,
                        principalTable: "Dispositivos",
                        principalColumn: "IdDispositivo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DispositivosIdDispositivo",
                table: "Sensors",
                column: "DispositivosIdDispositivo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leituras");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "Dispositivos");

            migrationBuilder.CreateTable(
                name: "Strings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strings", x => x.Id);
                });
        }
    }
}
