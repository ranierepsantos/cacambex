using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    public partial class InclusaoCampoNumeroCTRNoPedido : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cacambas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Volume = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    nCodServ = table.Column<long>(type: "bigint", nullable: false),
                    cCodIntServ = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cacambas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnderecosCobranca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CEP = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Logradouro = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Numero = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Complemento = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Bairro = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Cidade = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    UF = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecosCobranca", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quando = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Senha = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    Funcao = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo_cliente_integracao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Codigo_cliente_omie = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Documento = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime", nullable: false),
                    Telefone = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Contribuinte = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    EnderecoCobrancaId = table.Column<int>(type: "int", nullable: false),
                    Pessoa_fisica = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    TipoDocumento = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clientes_EnderecosCobranca_EnderecoCobrancaId",
                        column: x => x.EnderecoCobrancaId,
                        principalTable: "EnderecosCobranca",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PedidoItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VolumeCacamba = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    CacambaId = table.Column<int>(type: "int", nullable: true),
                    ValorUnitario = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    CTRId = table.Column<int>(type: "int", nullable: false),
                    RecolherItemId = table.Column<int>(type: "int", nullable: false),
                    ItemEntregueId = table.Column<int>(type: "int", nullable: false),
                    PedidoConcluidoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoItens_Cacambas_CacambaId",
                        column: x => x.CacambaId,
                        principalTable: "Cacambas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PedidoItens_Eventos_CTRId",
                        column: x => x.CTRId,
                        principalTable: "Eventos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PedidoItens_Eventos_ItemEntregueId",
                        column: x => x.ItemEntregueId,
                        principalTable: "Eventos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PedidoItens_Eventos_PedidoConcluidoId",
                        column: x => x.PedidoConcluidoId,
                        principalTable: "Eventos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PedidoItens_Eventos_RecolherItemId",
                        column: x => x.RecolherItemId,
                        principalTable: "Eventos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EnderecosEntrega",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CEP = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Logradouro = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Numero = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Complemento = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Bairro = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Cidade = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    UF = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecosEntrega", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecosEntrega_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    cCodIntOS = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nCodOS = table.Column<long>(type: "bigint", nullable: false),
                    NumeroNotaFiscal = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Observacao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    PedidoEmitidoId = table.Column<int>(type: "int", nullable: false),
                    TipoDePagamento = table.Column<int>(type: "int", nullable: false),
                    EnderecoEntregaId = table.Column<int>(type: "int", nullable: false),
                    PedidoItemId = table.Column<int>(type: "int", nullable: false),
                    ValorPedido = table.Column<decimal>(type: "decimal(12,2)", precision: 12, scale: 2, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    NotaFiscalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pedidos_EnderecosEntrega_EnderecoEntregaId",
                        column: x => x.EnderecoEntregaId,
                        principalTable: "EnderecosEntrega",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pedidos_Eventos_NotaFiscalId",
                        column: x => x.NotaFiscalId,
                        principalTable: "Eventos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pedidos_Eventos_PedidoEmitidoId",
                        column: x => x.PedidoEmitidoId,
                        principalTable: "Eventos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pedidos_PedidoItens_PedidoItemId",
                        column: x => x.PedidoItemId,
                        principalTable: "PedidoItens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_EnderecoCobrancaId",
                table: "Clientes",
                column: "EnderecoCobrancaId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecosEntrega_ClienteId",
                table: "EnderecosEntrega",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_CacambaId",
                table: "PedidoItens",
                column: "CacambaId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_CTRId",
                table: "PedidoItens",
                column: "CTRId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_ItemEntregueId",
                table: "PedidoItens",
                column: "ItemEntregueId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_PedidoConcluidoId",
                table: "PedidoItens",
                column: "PedidoConcluidoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_RecolherItemId",
                table: "PedidoItens",
                column: "RecolherItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ClienteId",
                table: "Pedidos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_EnderecoEntregaId",
                table: "Pedidos",
                column: "EnderecoEntregaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_NotaFiscalId",
                table: "Pedidos",
                column: "NotaFiscalId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_PedidoEmitidoId",
                table: "Pedidos",
                column: "PedidoEmitidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_PedidoItemId",
                table: "Pedidos",
                column: "PedidoItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "EnderecosEntrega");

            migrationBuilder.DropTable(
                name: "PedidoItens");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Cacambas");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "EnderecosCobranca");
        }
    }
}
