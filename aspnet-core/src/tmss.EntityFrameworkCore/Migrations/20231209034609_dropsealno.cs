using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class dropsealno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SealNo",
                table: "ProdContainerIntransit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SealNo",
                table: "ProdContainerIntransit",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }
    }
}
