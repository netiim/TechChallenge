using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class Ajustantochavesestrangeiras : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contato_CidadeId",
                table: "Contato");

            migrationBuilder.CreateIndex(
                name: "IX_Contato_CidadeId",
                table: "Contato",
                column: "CidadeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contato_CidadeId",
                table: "Contato");

            migrationBuilder.CreateIndex(
                name: "IX_Contato_CidadeId",
                table: "Contato",
                column: "CidadeId",
                unique: true);
        }
    }
}
