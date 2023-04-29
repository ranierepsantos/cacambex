using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    public partial class SeedData_TipoCacamba : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TipoCacambas",
                columns: new[] { "Id", "Ativo", "Preco", "Volume" },
                values: new object[] { 1, true, 100m, "3M³" });

            migrationBuilder.InsertData(
                table: "TipoCacambas",
                columns: new[] { "Id", "Ativo", "Preco", "Volume" },
                values: new object[] { 2, true, 200m, "5M³" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TipoCacambas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TipoCacambas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
