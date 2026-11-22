using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Migrations
{
    public partial class AutorUnico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Autores_Nombre_Apellido_Fecha_Nacimiento",
                table: "Autores",
                columns: new[] { "Nombre", "Apellido", "Fecha_Nacimiento" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Autores_Nombre_Apellido_Fecha_Nacimiento",
                table: "Autores");
        }
    }
}
