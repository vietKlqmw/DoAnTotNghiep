using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class orderpart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdOrderPart",
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
                    PartNo = table.Column<string>(maxLength: 15, nullable: true),
                    PartName = table.Column<string>(maxLength: 500, nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 10, nullable: true),
                    CarfamilyCode = table.Column<string>(maxLength: 4, nullable: true),
                    SupplierId = table.Column<long>(nullable: true),
                    MaterialId = table.Column<long>(nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    Remark = table.Column<string>(maxLength: 5000, nullable: true),
                    ShipmentId = table.Column<long>(nullable: true),
                    ContainerNo = table.Column<string>(maxLength: 20, nullable: true),
                    Qty = table.Column<int>(nullable: true),
                    AmountUnit = table.Column<decimal>(nullable: true),
                    TotalAmount = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdOrderPart", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdOrderPart");
        }
    }
}
