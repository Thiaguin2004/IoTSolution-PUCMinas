using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTSolution.Migrations
{
    /// <inheritdoc />
    public partial class eigth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Usuarios_UsuariosModelIdUsuario",
                table: "Dispositivos");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Dispositivos_DispositivosModelIdDispositivo",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_DispositivosModelIdDispositivo",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivos_UsuariosModelIdUsuario",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "DispositivosModelIdDispositivo",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "UsuariosModelIdUsuario",
                table: "Dispositivos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DispositivosModelIdDispositivo",
                table: "Sensors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuariosModelIdUsuario",
                table: "Dispositivos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DispositivosModelIdDispositivo",
                table: "Sensors",
                column: "DispositivosModelIdDispositivo");

            migrationBuilder.CreateIndex(
                name: "IX_Dispositivos_UsuariosModelIdUsuario",
                table: "Dispositivos",
                column: "UsuariosModelIdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Dispositivos_Usuarios_UsuariosModelIdUsuario",
                table: "Dispositivos",
                column: "UsuariosModelIdUsuario",
                principalTable: "Usuarios",
                principalColumn: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Dispositivos_DispositivosModelIdDispositivo",
                table: "Sensors",
                column: "DispositivosModelIdDispositivo",
                principalTable: "Dispositivos",
                principalColumn: "IdDispositivo");
        }
    }
}
