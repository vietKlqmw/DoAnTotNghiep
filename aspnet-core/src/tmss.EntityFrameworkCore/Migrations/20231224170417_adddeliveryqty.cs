using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class adddeliveryqty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalDeliveryQty",
                table: "ProdInvoiceStockOut",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDeliveryQty",
                table: "ProdInvoiceStockOut");
        }
    }
}
