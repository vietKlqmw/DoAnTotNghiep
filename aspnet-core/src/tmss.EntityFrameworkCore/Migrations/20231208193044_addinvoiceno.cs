using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class addinvoiceno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "ProdInvoiceDetails");

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

            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "ProdInvoiceDetails",
                type: "bigint",
                nullable: true);
        }
    }
}
