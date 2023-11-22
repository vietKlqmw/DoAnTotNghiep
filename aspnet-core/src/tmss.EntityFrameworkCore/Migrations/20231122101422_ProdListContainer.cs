using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class ProdListContainer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdContainerList",
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
                    SupplierNo = table.Column<string>(maxLength: 10, nullable: true),
                    BillOfLadingNo = table.Column<string>(maxLength: 20, nullable: true),
                    SealNo = table.Column<string>(maxLength: 20, nullable: true),
                    ContainerSize = table.Column<int>(nullable: true),
                    ShipmentId = table.Column<long>(nullable: true),
                    ShippingDate = table.Column<DateTime>(nullable: true),
                    PortDate = table.Column<DateTime>(nullable: true),
                    PortDateActual = table.Column<DateTime>(nullable: true),
                    PortTransitDate = table.Column<DateTime>(nullable: true),
                    ReceiveDate = table.Column<DateTime>(nullable: true),
                    RequestId = table.Column<long>(nullable: true),
                    InvoiceNo = table.Column<string>(maxLength: 20, nullable: true),
                    ListLotNo = table.Column<string>(maxLength: 1000, nullable: true),
                    ListCaseNo = table.Column<string>(maxLength: 1000, nullable: true),
                    Transport = table.Column<string>(maxLength: 10, nullable: true),
                    DevanningDate = table.Column<DateTime>(nullable: true),
                    DevanningTime = table.Column<string>(maxLength: 255, nullable: true),
                    Remark = table.Column<string>(maxLength: 1000, nullable: true),
                    WhLocation = table.Column<string>(maxLength: 10, nullable: true),
                    GateInDate = table.Column<DateTime>(nullable: true),
                    GateInTime = table.Column<string>(maxLength: 255, nullable: true),
                    TransitPortReqId = table.Column<long>(nullable: true),
                    TransitPortReqDate = table.Column<DateTime>(nullable: true),
                    TransitPortReqTime = table.Column<string>(maxLength: 255, nullable: true),
                    Freight = table.Column<decimal>(nullable: true),
                    Insurance = table.Column<decimal>(nullable: true),
                    Cif = table.Column<decimal>(nullable: true),
                    Tax = table.Column<decimal>(nullable: true),
                    Amount = table.Column<decimal>(nullable: true),
                    Status = table.Column<string>(maxLength: 20, nullable: true),
                    LocationCode = table.Column<string>(maxLength: 20, nullable: true),
                    LocationDate = table.Column<DateTime>(nullable: true),
                    ReceivingPeriodId = table.Column<long>(nullable: true),
                    OrdertypeCode = table.Column<string>(maxLength: 4, nullable: true),
                    GoodstypeCode = table.Column<string>(maxLength: 4, nullable: true),
                    RentalWhId = table.Column<long>(nullable: true),
                    RequestStatus = table.Column<string>(maxLength: 20, nullable: true),
                    BillDate = table.Column<DateTime>(nullable: true),
                    ReceiveTime = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdContainerList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProdContainerTransitPortPlan",
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
                    RequestDate = table.Column<DateTime>(nullable: true),
                    RequestTime = table.Column<TimeSpan>(nullable: true),
                    InvoiceNo = table.Column<string>(maxLength: 20, nullable: true),
                    BillOfLadingNo = table.Column<string>(maxLength: 20, nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 10, nullable: true),
                    SealNo = table.Column<string>(maxLength: 20, nullable: true),
                    ListCaseNo = table.Column<string>(maxLength: 1000, nullable: true),
                    ListLotNo = table.Column<string>(maxLength: 1000, nullable: true),
                    Transport = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(maxLength: 10, nullable: true),
                    Customs1 = table.Column<string>(maxLength: 100, nullable: true),
                    Customs2 = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdContainerTransitPortPlan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProdContainerTransitPortPlan_T",
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
                    Guid = table.Column<string>(maxLength: 128, nullable: true),
                    ContainerNo = table.Column<string>(maxLength: 15, nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: true),
                    RequestTime = table.Column<TimeSpan>(nullable: true),
                    InvoiceNo = table.Column<string>(maxLength: 20, nullable: true),
                    BillOfLadingNo = table.Column<string>(maxLength: 20, nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 10, nullable: true),
                    SealNo = table.Column<string>(maxLength: 20, nullable: true),
                    ListCaseNo = table.Column<string>(maxLength: 1000, nullable: true),
                    ListLotNo = table.Column<string>(maxLength: 1000, nullable: true),
                    Transport = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(maxLength: 10, nullable: true),
                    Customs1 = table.Column<string>(maxLength: 100, nullable: true),
                    Customs2 = table.Column<string>(maxLength: 100, nullable: true),
                    ErrorDescription = table.Column<string>(maxLength: 5000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdContainerTransitPortPlan_T", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdContainerList");

            migrationBuilder.DropTable(
                name: "ProdContainerTransitPortPlan");

            migrationBuilder.DropTable(
                name: "ProdContainerTransitPortPlan_T");
        }
    }
}
