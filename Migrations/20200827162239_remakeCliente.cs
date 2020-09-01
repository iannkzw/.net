using Microsoft.EntityFrameworkCore.Migrations;

namespace CRUD.Migrations
{
    public partial class remakeCliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Produto",
                table: "Clientes",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Valor",
                table: "Clientes",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Produto",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Valor",
                table: "Clientes");
        }
    }
}
