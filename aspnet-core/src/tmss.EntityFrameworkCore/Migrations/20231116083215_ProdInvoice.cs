using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class ProdInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdContainerInvoice",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    ContainerNo = table.Column<string>(maxLength: 15, nullable: true),
                    InvoiceId = table.Column<long>(nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 10, nullable: true),
                    SealNo = table.Column<string>(maxLength: 20, nullable: true),
                    ContainerSize = table.Column<int>(nullable: true),
                    PlandedvanningDate = table.Column<DateTime>(nullable: true),
                    ActualvanningDate = table.Column<DateTime>(nullable: true),
                    Thc = table.Column<decimal>(nullable: true),
                    Status = table.Column<string>(maxLength: 10, nullable: true),
                    ThcVn = table.Column<decimal>(nullable: true),
                    PeriodDate = table.Column<DateTime>(nullable: true),
                    PeriodId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdContainerInvoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProdInvoice",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    InvoiceNo = table.Column<string>(maxLength: 20, nullable: true),
                    BillId = table.Column<long>(nullable: true),
                    OrderTypeCode = table.Column<string>(maxLength: 4, nullable: true),
                    GoodsTypeCode = table.Column<string>(maxLength: 4, nullable: true),
                    InvoiceParentId = table.Column<long>(nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: true),
                    Freight = table.Column<decimal>(nullable: true),
                    FreightTotal = table.Column<decimal>(nullable: true),
                    Insurance = table.Column<decimal>(nullable: true),
                    InsuranceTotal = table.Column<decimal>(nullable: true),
                    Cif = table.Column<decimal>(nullable: true),
                    ThcTotal = table.Column<decimal>(nullable: true),
                    NetWeight = table.Column<decimal>(nullable: true),
                    GrossWeight = table.Column<decimal>(nullable: true),
                    Currency = table.Column<string>(maxLength: 20, nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 10, nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    Status = table.Column<string>(maxLength: 10, nullable: true),
                    FreightTotalVn = table.Column<decimal>(nullable: true),
                    InsuranceTotalVn = table.Column<decimal>(nullable: true),
                    CifVn = table.Column<decimal>(nullable: true),
                    ThcTotalVn = table.Column<decimal>(nullable: true),
                    PeriodId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdInvoice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProdInvoiceDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    PartNo = table.Column<string>(maxLength: 12, nullable: true),
                    LotNo = table.Column<string>(maxLength: 10, nullable: true),
                    Fixlot = table.Column<string>(maxLength: 4, nullable: true),
                    CaseNo = table.Column<string>(maxLength: 30, nullable: true),
                    ModuleNo = table.Column<string>(maxLength: 30, nullable: true),
                    Insurance = table.Column<decimal>(nullable: true),
                    ContainerNo = table.Column<string>(maxLength: 15, nullable: true),
                    InvoiceId = table.Column<long>(nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 50, nullable: true),
                    Freight = table.Column<decimal>(nullable: true),
                    Thc = table.Column<decimal>(nullable: true),
                    Cif = table.Column<decimal>(nullable: true),
                    Tax = table.Column<decimal>(nullable: true),
                    TaxRate = table.Column<decimal>(nullable: true),
                    Vat = table.Column<decimal>(nullable: true),
                    VatRate = table.Column<decimal>(nullable: true),
                    UsageQty = table.Column<int>(nullable: true),
                    PartName = table.Column<string>(maxLength: 300, nullable: true),
                    CarfamilyCode = table.Column<string>(maxLength: 4, nullable: true),
                    PartNetWeight = table.Column<decimal>(nullable: true),
                    OrderNo = table.Column<string>(maxLength: 12, nullable: true),
                    PackagingDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(maxLength: 10, nullable: true),
                    FreightVn = table.Column<decimal>(nullable: true),
                    InsuranceVn = table.Column<decimal>(nullable: true),
                    ThcVn = table.Column<decimal>(nullable: true),
                    CifVn = table.Column<decimal>(nullable: true),
                    TaxVn = table.Column<decimal>(nullable: true),
                    VatVn = table.Column<decimal>(nullable: true),
                    InvoiceParentId = table.Column<long>(nullable: true),
                    PeriodDate = table.Column<DateTime>(nullable: true),
                    PeriodId = table.Column<decimal>(nullable: true),
                    PartnameVn = table.Column<string>(maxLength: 300, nullable: true),
                    CarName = table.Column<string>(maxLength: 200, nullable: true),
                    PreCustomsId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdInvoiceDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdContainerInvoice");

            migrationBuilder.DropTable(
                name: "ProdInvoice");

            migrationBuilder.DropTable(
                name: "ProdInvoiceDetails");
        }
    }
}
