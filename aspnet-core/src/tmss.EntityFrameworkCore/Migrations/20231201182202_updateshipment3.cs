using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class updateshipment3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ProdShipment",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProdShipment");
        }
    }
}
