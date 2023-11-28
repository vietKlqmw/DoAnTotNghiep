using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class UpdateImportContWH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DevanningTime",
                table: "ProdContainerRentalWHPlan_T",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "DevanningTime",
                table: "ProdContainerRentalWHPlan_T",
                type: "time",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
