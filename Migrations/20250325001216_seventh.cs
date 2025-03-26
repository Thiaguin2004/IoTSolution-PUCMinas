using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IoTSolution.Migrations
{
    /// <inheritdoc />
    public partial class seventh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsuariosModelIdUsuario",
                table: "Dispositivos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sobrenome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CPF = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dispositivos_Usuarios_UsuariosModelIdUsuario",
                table: "Dispositivos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Dispositivos_UsuariosModelIdUsuario",
                table: "Dispositivos");

            migrationBuilder.DropColumn(
                name: "UsuariosModelIdUsuario",
                table: "Dispositivos");
        }
    }
}
