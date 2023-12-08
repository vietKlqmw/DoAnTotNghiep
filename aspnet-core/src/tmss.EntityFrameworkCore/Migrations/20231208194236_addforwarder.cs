using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class addforwarder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Forwarder",
                table: "ProdContainerIntransit");

            migrationBuilder.AddColumn<string>(
                name: "Forwarder",
                table: "ProdShipment",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Forwarder",
                table: "ProdShipment");

            migrationBuilder.AddColumn<string>(
                name: "Forwarder",
                table: "ProdContainerIntransit",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }
    }
}
