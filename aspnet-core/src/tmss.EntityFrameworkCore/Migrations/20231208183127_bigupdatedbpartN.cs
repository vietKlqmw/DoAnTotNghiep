using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class bigupdatedbpartN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CifVn",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "FreightVn",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "InsuranceVn",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "PackagingDate",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "PartNetWeight",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "PartnameVn",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "TaxVn",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "ThcVn",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "VatVn",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "Cif",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "CifVn",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "Freight",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "FreightTotal",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "FreightTotalVn",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "GrossWeight",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "Insurance",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "InsuranceTotal",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "InsuranceTotalVn",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "NetWeight",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "SupplierNo",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "ThcTotal",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "ThcTotalVn",
                table: "ProdInvoice");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "ProdInvoiceDetails",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GrossWeight",
                table: "ProdInvoiceDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SealNo",
                table: "ProdInvoiceDetails",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Forwarder",
                table: "ProdInvoice",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PartListId",
                table: "ProdContainerIntransit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "GrossWeight",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "SealNo",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "Forwarder",
                table: "ProdInvoice");

            migrationBuilder.DropColumn(
                name: "PartListId",
                table: "ProdContainerIntransit");

            migrationBuilder.AddColumn<decimal>(
                name: "CifVn",
                table: "ProdInvoiceDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FreightVn",
                table: "ProdInvoiceDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InsuranceVn",
                table: "ProdInvoiceDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PackagingDate",
                table: "ProdInvoiceDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PartNetWeight",
                table: "ProdInvoiceDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PartnameVn",
                table: "ProdInvoiceDetails",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ProdInvoiceDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxVn",
                table: "ProdInvoiceDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ThcVn",
                table: "ProdInvoiceDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "VatVn",
                table: "ProdInvoiceDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Cif",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CifVn",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "ProdInvoice",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Freight",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FreightTotal",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FreightTotalVn",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GrossWeight",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Insurance",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InsuranceTotal",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InsuranceTotalVn",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "NetWeight",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "ProdInvoice",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierNo",
                table: "ProdInvoice",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ThcTotal",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ThcTotalVn",
                table: "ProdInvoice",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
