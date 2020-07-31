using Microsoft.EntityFrameworkCore.Migrations;

namespace NubeSalesMVC.Migrations
{
    public partial class segunda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdTipoReceita",
                table: "Receber",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "Receber",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdTipoDespesa",
                table: "Pagar",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "Pagar",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdTipoReceita",
                table: "Receber");

            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "Receber");

            migrationBuilder.DropColumn(
                name: "IdTipoDespesa",
                table: "Pagar");

            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "Pagar");
        }
    }
}
