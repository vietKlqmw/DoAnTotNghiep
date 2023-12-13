using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class addorderedqty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNoOut",
                table: "ProdStockReceiving",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderedQty",
                table: "ProdStockReceiving",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderedQty",
                table: "ProdStockReceiving");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceNoOut",
                table: "ProdStockReceiving",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
