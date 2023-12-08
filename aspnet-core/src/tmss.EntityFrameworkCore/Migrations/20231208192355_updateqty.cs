using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class updateqty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceNo",
                table: "ProdInvoiceDetails");

            migrationBuilder.AddColumn<int>(
                name: "UsageQty",
                table: "ProdContainerIntransit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsageQty",
                table: "ProdContainerIntransit");

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNo",
                table: "ProdInvoiceDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
