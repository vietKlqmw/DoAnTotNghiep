using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class ProdContainerRentalWHPlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdContainerRentalWHPlan",
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
                    RequestDate = table.Column<DateTime>(type: "date", nullable: true),
                    RequestTime = table.Column<TimeSpan>(type: "time(7)", nullable: true),
                    InvoiceNo = table.Column<string>(maxLength: 20, nullable: true),
                    BillofladingNo = table.Column<string>(maxLength: 20, nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 10, nullable: true),
                    SealNo = table.Column<string>(maxLength: 20, nullable: true),
                    ListcaseNo = table.Column<string>(maxLength: 1000, nullable: true),
                    ListLotNo = table.Column<string>(maxLength: 1000, nullable: true),
                    DevanningDate = table.Column<DateTime>(nullable: true),
                    DevanningTime = table.Column<TimeSpan>(type: "time(7)", nullable: true),
                    ActualDevanningDate = table.Column<DateTime>(nullable: true),
                    GateInPlanTime = table.Column<DateTime>(nullable: true),
                    GateInActualDateTime = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    Transport = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdContainerRentalWHPlan", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdContainerRentalWHPlan");
        }
    }
}
