using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class upwh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StorageLocation",
                table: "MasterStorageLocation",
                maxLength: 3,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Inventory",
                table: "MasterStorageLocation",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxStock",
                table: "MasterStorageLocation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MasterStorageLocation",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inventory",
                table: "MasterStorageLocation");

            migrationBuilder.DropColumn(
                name: "MaxStock",
                table: "MasterStorageLocation");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MasterStorageLocation");

            migrationBuilder.AlterColumn<string>(
                name: "StorageLocation",
                table: "MasterStorageLocation",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3,
                oldNullable: true);
        }
    }
}
