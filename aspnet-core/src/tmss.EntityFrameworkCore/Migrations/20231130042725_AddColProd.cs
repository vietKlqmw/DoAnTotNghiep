using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class AddColProd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TmvDate",
                table: "ProdContainerIntransit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "ProdContainerIntransit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TmvDate",
                table: "ProdContainerIntransit");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "ProdContainerIntransit");
        }
    }
}
