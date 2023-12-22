using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class adduomorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseUnitOfMeasure",
                table: "ProdOrderPart",
                maxLength: 3,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseUnitOfMeasure",
                table: "ProdOrderPart");
        }
    }
}
