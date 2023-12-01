using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class ProdInvoiceDetailsUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InvoiceNo",
                table: "ProdInvoiceDetails",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceNo",
                table: "ProdInvoiceDetails");
        }
    }
}
