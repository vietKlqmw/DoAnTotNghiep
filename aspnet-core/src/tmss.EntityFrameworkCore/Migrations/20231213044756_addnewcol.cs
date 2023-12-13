using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class addnewcol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionDatetime",
                table: "ProdStockReceiving");

            migrationBuilder.DropColumn(
                name: "WorkingDate",
                table: "ProdStockReceiving");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "ProdStockReceiving",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "ProdStockReceiving",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestStatus",
                table: "ProdStockReceiving",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Warehouse",
                table: "ProdStockReceiving",
                maxLength: 2,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "ProdStockReceiving");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "ProdStockReceiving");

            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "ProdStockReceiving");

            migrationBuilder.DropColumn(
                name: "Warehouse",
                table: "ProdStockReceiving");

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDatetime",
                table: "ProdStockReceiving",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkingDate",
                table: "ProdStockReceiving",
                type: "datetime2",
                nullable: true);
        }
    }
}
