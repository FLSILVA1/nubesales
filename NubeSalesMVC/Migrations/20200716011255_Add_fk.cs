using Microsoft.EntityFrameworkCore.Migrations;

namespace NubeSalesMVC.Migrations
{
    public partial class Add_fk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdPessoa",
                table: "Receber",
                newName: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Receber_PessoaId",
                table: "Receber",
                column: "PessoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receber_Pessoa_PessoaId",
                table: "Receber",
                column: "PessoaId",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receber_Pessoa_PessoaId",
                table: "Receber");

            migrationBuilder.DropIndex(
                name: "IX_Receber_PessoaId",
                table: "Receber");

            migrationBuilder.RenameColumn(
                name: "PessoaId",
                table: "Receber",
                newName: "IdPessoa");
        }
    }
}
