using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class updatetempl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CarfamilyCode",
                table: "ProdContainerIntransit_T",
                maxLength: 4,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarfamilyCode",
                table: "ProdContainerIntransit_T");
        }
    }
}
