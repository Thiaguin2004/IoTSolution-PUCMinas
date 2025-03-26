using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTSolution.Migrations
{
    /// <inheritdoc />
    public partial class sixth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dispositivo",
                table: "Sensors",
                newName: "IdDispositivo");

            migrationBuilder.AddColumn<int>(
                name: "DispositivosModelIdDispositivo",
                table: "Sensors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Dispositivos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DispositivosModelIdDispositivo",
                table: "Sensors",
                column: "DispositivosModelIdDispositivo");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Dispositivos_DispositivosModelIdDispositivo",
                table: "Sensors",
                column: "DispositivosModelIdDispositivo",
                principalTable: "Dispositivos",
                principalColumn: "IdDispositivo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Dispositivos_DispositivosModelIdDispositivo",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_DispositivosModelIdDispositivo",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "DispositivosModelIdDispositivo",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Dispositivos");

            migrationBuilder.RenameColumn(
                name: "IdDispositivo",
                table: "Sensors",
                newName: "Dispositivo");
        }
    }
}
