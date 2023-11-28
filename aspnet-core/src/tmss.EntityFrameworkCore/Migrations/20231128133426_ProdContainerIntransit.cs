using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class ProdContainerIntransit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdContainerIntransit",
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
                    ShippingDate = table.Column<DateTime>(nullable: true),
                    PortDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(maxLength: 10, nullable: true),
                    Forwarder = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdContainerIntransit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProdStockPart",
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
                    PartNoNormalized = table.Column<string>(maxLength: 12, nullable: true),
                    PartName = table.Column<string>(maxLength: 300, nullable: true),
                    PartNoNormalizedS4 = table.Column<string>(maxLength: 10, nullable: true),
                    PartListId = table.Column<long>(nullable: true),
                    MaterialId = table.Column<long>(nullable: true),
                    Qty = table.Column<decimal>(nullable: true),
                    WorkingDate = table.Column<DateTime>(nullable: true),
                    LastCalDatetime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdStockPart", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdContainerIntransit");

            migrationBuilder.DropTable(
                name: "ProdStockPart");
        }
    }
}
