using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    public partial class CreateTable_PrecoFaixaCep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrecoFaixaCeps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoCacambaId = table.Column<int>(type: "int", nullable: false),
                    CepInicial = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false),
                    CepFinal = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrecoFaixaCeps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrecoFaixaCeps_TipoCacambas_TipoCacambaId",
                        column: x => x.TipoCacambaId,
                        principalTable: "TipoCacambas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrecoFaixaCeps_TipoCacambaId",
                table: "PrecoFaixaCeps",
                column: "TipoCacambaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrecoFaixaCeps");
        }
    }
}
