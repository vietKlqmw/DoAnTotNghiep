using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class addgrnNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoodsReceivedNoteNo",
                table: "ProdContainerRentalWHPlan",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoodsReceivedNoteNo",
                table: "ProdContainerRentalWHPlan");
        }
    }
}
