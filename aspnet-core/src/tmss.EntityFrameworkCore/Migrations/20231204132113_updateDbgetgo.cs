using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class updateDbgetgo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarName",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "CaseNo",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "Fixlot",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "InvoiceParentId",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "LotNo",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "PeriodDate",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "PreCustomsId",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "GoodsTypeCode",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "InvoiceParentId",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "OrderTypeCode",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "ProdInvoice");

            migrationBuilder.AddColumn<long>(
                name: "ShipmentId",
                table: "ProdContainerIntransit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "ProdContainerIntransit");

            migrationBuilder.AddColumn<string>(
                name: "CarName",
                table: "ProdInvoiceDetails",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CaseNo",
                table: "ProdInvoiceDetails",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fixlot",
                table: "ProdInvoiceDetails",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceParentId",
                table: "ProdInvoiceDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotNo",
                table: "ProdInvoiceDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderNo",
                table: "ProdInvoiceDetails",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodDate",
                table: "ProdInvoiceDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PeriodId",
                table: "ProdInvoiceDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PreCustomsId",
                table: "ProdInvoiceDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoodsTypeCode",
                table: "ProdInvoice",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceParentId",
                table: "ProdInvoice",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderTypeCode",
                table: "ProdInvoice",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PeriodId",
                table: "ProdInvoice",
                type: "bigint",
                nullable: true);
        }
    }
}
