using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class updateInvoiceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "ProdCustomsDeclare",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "ProdCustomsDeclare");
        }
    }
}
