using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class AddErrorDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorDescription",
                table: "MasterVehicleCKD_T",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorDescription",
                table: "MasterMaterial_T",
                maxLength: 5000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorDescription",
                table: "MasterEngine_T",
                maxLength: 5000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorDescription",
                table: "MasterVehicleCKD_T");

            migrationBuilder.DropColumn(
                name: "ErrorDescription",
                table: "MasterMaterial_T");

            migrationBuilder.DropColumn(
                name: "ErrorDescription",
                table: "MasterEngine_T");
        }
    }
}
