using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTSolution.Migrations
{
    /// <inheritdoc />
    public partial class fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Dispositivos_DispositivosIdDispositivo",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_DispositivosIdDispositivo",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "DispositivosIdDispositivo",
                table: "Sensors");

            migrationBuilder.AddColumn<int>(
                name: "Dispositivo",
                table: "Sensors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dispositivo",
                table: "Sensors");

            migrationBuilder.AddColumn<int>(
                name: "DispositivosIdDispositivo",
                table: "Sensors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DispositivosIdDispositivo",
                table: "Sensors",
                column: "DispositivosIdDispositivo");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Dispositivos_DispositivosIdDispositivo",
                table: "Sensors",
                column: "DispositivosIdDispositivo",
                principalTable: "Dispositivos",
                principalColumn: "IdDispositivo");
        }
    }
}
