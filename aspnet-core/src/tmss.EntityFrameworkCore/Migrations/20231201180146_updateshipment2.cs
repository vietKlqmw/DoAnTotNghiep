using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class updateshipment2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProdShipment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ProdShipment",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
