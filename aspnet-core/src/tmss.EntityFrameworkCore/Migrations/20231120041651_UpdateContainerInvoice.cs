using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class UpdateContainerInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualvanningDate",
                table: "ProdContainerInvoice");

            migrationBuilder.DropColumn(
                name: "PeriodDate",
                table: "ProdContainerInvoice");

            migrationBuilder.DropColumn(
                name: "PeriodId",
                table: "ProdContainerInvoice");

            migrationBuilder.DropColumn(
                name: "PlandedvanningDate",
                table: "ProdContainerInvoice");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDevanningDate",
                table: "ProdContainerInvoice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "ProdContainerInvoice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Freight",
                table: "ProdContainerInvoice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Insurance",
                table: "ProdContainerInvoice",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlanDevanningDate",
                table: "ProdContainerInvoice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "ProdContainerInvoice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxVnd",
                table: "ProdContainerInvoice",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VatVnd",
                table: "ProdContainerInvoice",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualDevanningDate",
                table: "ProdContainerInvoice");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ProdContainerInvoice");

            migrationBuilder.DropColumn(
                name: "Freight",
                table: "ProdContainerInvoice");

            migrationBuilder.DropColumn(
                name: "Insurance",
                table: "ProdContainerInvoice");

            migrationBuilder.DropColumn(
                name: "PlanDevanningDate",
                table: "ProdContainerInvoice");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "ProdContainerInvoice");

            migrationBuilder.DropColumn(
                name: "TaxVnd",
                table: "ProdContainerInvoice");

            migrationBuilder.DropColumn(
                name: "VatVnd",
                table: "ProdContainerInvoice");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualvanningDate",
                table: "ProdContainerInvoice",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PeriodDate",
                table: "ProdContainerInvoice",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PeriodId",
                table: "ProdContainerInvoice",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlandedvanningDate",
                table: "ProdContainerInvoice",
                type: "datetime2",
                nullable: true);
        }
    }
}
