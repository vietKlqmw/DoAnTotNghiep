using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class changedatatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ListStockId",
                table: "ProdInvoiceStockOut",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ListStockId",
                table: "ProdInvoiceStockOut",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
