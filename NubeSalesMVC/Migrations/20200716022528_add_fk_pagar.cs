using Microsoft.EntityFrameworkCore.Migrations;

namespace NubeSalesMVC.Migrations
{
    public partial class add_fk_pagar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdPessoa",
                table: "Pagar",
                newName: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagar_PessoaId",
                table: "Pagar",
                column: "PessoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagar_Pessoa_PessoaId",
                table: "Pagar",
                column: "PessoaId",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagar_Pessoa_PessoaId",
                table: "Pagar");

            migrationBuilder.DropIndex(
                name: "IX_Pagar_PessoaId",
                table: "Pagar");

            migrationBuilder.RenameColumn(
                name: "PessoaId",
                table: "Pagar",
                newName: "IdPessoa");
        }
    }
}
