using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class relacionamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contato_CidadeId",
                table: "Contato",
                column: "CidadeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cidade_EstadoId",
                table: "Cidade",
                column: "EstadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cidade_Estado_EstadoId",
                table: "Cidade",
                column: "EstadoId",
                principalTable: "Estado",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contato_Cidade_CidadeId",
                table: "Contato",
                column: "CidadeId",
                principalTable: "Cidade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cidade_Estado_EstadoId",
                table: "Cidade");

            migrationBuilder.DropForeignKey(
                name: "FK_Contato_Cidade_CidadeId",
                table: "Contato");

            migrationBuilder.DropIndex(
                name: "IX_Contato_CidadeId",
                table: "Contato");

            migrationBuilder.DropIndex(
                name: "IX_Cidade_EstadoId",
                table: "Cidade");
        }
    }
}
