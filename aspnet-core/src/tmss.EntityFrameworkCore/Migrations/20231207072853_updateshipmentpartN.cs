using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class updateshipmentpartN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProdContainerIntransit");

            migrationBuilder.DropColumn(
                name: "TmvDate",
                table: "ProdContainerIntransit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ProdContainerIntransit",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TmvDate",
                table: "ProdContainerIntransit",
                type: "datetime2",
                nullable: true);
        }
    }
}
