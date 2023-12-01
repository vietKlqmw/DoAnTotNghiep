using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class UpdateStockPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "ProdStockPart",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierNo",
                table: "ProdStockPart",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Model",
                table: "ProdStockPart");

            migrationBuilder.DropColumn(
                name: "SupplierNo",
                table: "ProdStockPart");
        }
    }
}
