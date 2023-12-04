using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class updateforwarder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingNo",
                table: "MasterForwarder");

            migrationBuilder.DropColumn(
                name: "SupplierNo",
                table: "MasterForwarder");

            migrationBuilder.AddColumn<long>(
                name: "SupplierId",
                table: "MasterForwarder",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "MasterForwarder");

            migrationBuilder.AddColumn<string>(
                name: "ShippingNo",
                table: "MasterForwarder",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierNo",
                table: "MasterForwarder",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
