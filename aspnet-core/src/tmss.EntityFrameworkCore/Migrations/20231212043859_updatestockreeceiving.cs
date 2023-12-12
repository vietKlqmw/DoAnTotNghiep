using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class updatestockreeceiving : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActualQty",
                table: "ProdStockReceiving",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContainerNo",
                table: "ProdStockReceiving",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNoOut",
                table: "ProdStockReceiving",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderQty",
                table: "ProdStockReceiving",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualQty",
                table: "ProdStockReceiving");

            migrationBuilder.DropColumn(
                name: "ContainerNo",
                table: "ProdStockReceiving");

            migrationBuilder.DropColumn(
                name: "InvoiceNoOut",
                table: "ProdStockReceiving");

            migrationBuilder.DropColumn(
                name: "OrderQty",
                table: "ProdStockReceiving");
        }
    }
}
