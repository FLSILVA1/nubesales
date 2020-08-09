using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NubeSalesMVC.Migrations
{
    public partial class cateoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdTipoReceita",
                table: "Receber",
                newName: "CategoriaId");

            migrationBuilder.AddColumn<bool>(
                name: "IdFinPagar",
                table: "Pessoa",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IdFinReceber",
                table: "Pessoa",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    IntPagar = table.Column<bool>(nullable: false),
                    IntReceber = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receber_CategoriaId",
                table: "Receber",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receber_Categoria_CategoriaId",
                table: "Receber",
                column: "CategoriaId",
                principalTable: "Categoria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receber_Categoria_CategoriaId",
                table: "Receber");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropIndex(
                name: "IX_Receber_CategoriaId",
                table: "Receber");

            migrationBuilder.DropColumn(
                name: "IdFinPagar",
                table: "Pessoa");

            migrationBuilder.DropColumn(
                name: "IdFinReceber",
                table: "Pessoa");

            migrationBuilder.RenameColumn(
                name: "CategoriaId",
                table: "Receber",
                newName: "IdTipoReceita");
        }
    }
}
