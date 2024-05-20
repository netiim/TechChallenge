using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class alterandologicacidadeporregiao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contato_Cidade_CidadeId",
                table: "Contato");

            migrationBuilder.DropTable(
                name: "Cidade");

            migrationBuilder.RenameColumn(
                name: "CidadeId",
                table: "Contato",
                newName: "RegiaoId");

            migrationBuilder.RenameIndex(
                name: "IX_Contato_CidadeId",
                table: "Contato",
                newName: "IX_Contato_RegiaoId");

            migrationBuilder.CreateTable(
                name: "Regiao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numeroDDD = table.Column<int>(type: "INT", nullable: false),
                    EstadoId = table.Column<int>(type: "INT", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regiao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Regiao_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Regiao_EstadoId",
                table: "Regiao",
                column: "EstadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contato_Regiao_RegiaoId",
                table: "Contato",
                column: "RegiaoId",
                principalTable: "Regiao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contato_Regiao_RegiaoId",
                table: "Contato");

            migrationBuilder.DropTable(
                name: "Regiao");

            migrationBuilder.RenameColumn(
                name: "RegiaoId",
                table: "Contato",
                newName: "CidadeId");

            migrationBuilder.RenameIndex(
                name: "IX_Contato_RegiaoId",
                table: "Contato",
                newName: "IX_Contato_CidadeId");

            migrationBuilder.CreateTable(
                name: "Cidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INT", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstadoId = table.Column<int>(type: "INT", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    numeroDDD = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cidade_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cidade_EstadoId",
                table: "Cidade",
                column: "EstadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contato_Cidade_CidadeId",
                table: "Contato",
                column: "CidadeId",
                principalTable: "Cidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
