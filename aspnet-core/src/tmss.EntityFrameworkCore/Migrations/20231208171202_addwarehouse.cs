using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class addwarehouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ToWarehouse",
                table: "ProdCustomsDeclare",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToWarehouse",
                table: "ProdCustomsDeclare");
        }
    }
}
