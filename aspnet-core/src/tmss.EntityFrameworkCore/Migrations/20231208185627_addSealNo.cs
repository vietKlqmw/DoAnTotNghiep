using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class addSealNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SealNo",
                table: "ProdInvoiceDetails");

            migrationBuilder.AddColumn<string>(
                name: "SealNo",
                table: "ProdContainerIntransit",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SealNo",
                table: "ProdContainerIntransit");

            migrationBuilder.AddColumn<string>(
                name: "SealNo",
                table: "ProdInvoiceDetails",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
