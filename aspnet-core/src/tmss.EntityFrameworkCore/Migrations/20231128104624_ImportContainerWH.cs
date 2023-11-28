using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class ImportContainerWH : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdContainerRentalWHPlan_T",
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
                    BillofladingNo = table.Column<string>(maxLength: 20, nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 10, nullable: true),
                    SealNo = table.Column<string>(maxLength: 20, nullable: true),
                    ListcaseNo = table.Column<string>(maxLength: 1000, nullable: true),
                    ListLotNo = table.Column<string>(maxLength: 1000, nullable: true),
                    DevanningDate = table.Column<DateTime>(nullable: true),
                    DevanningTime = table.Column<TimeSpan>(nullable: true),
                    ActualDevanningDate = table.Column<DateTime>(nullable: true),
                    GateInPlanTime = table.Column<DateTime>(nullable: true),
                    GateInActualDateTime = table.Column<DateTime>(nullable: true),
                    Transport = table.Column<string>(maxLength: 50, nullable: true),
                    Status = table.Column<string>(maxLength: 10, nullable: true),
                    ErrorDescription = table.Column<string>(maxLength: 5000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdContainerRentalWHPlan_T", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdContainerRentalWHPlan_T");
        }
    }
}
