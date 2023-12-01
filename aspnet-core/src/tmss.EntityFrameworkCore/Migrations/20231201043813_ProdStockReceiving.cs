using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class ProdStockReceiving : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdStockPart");

            migrationBuilder.CreateTable(
                name: "ProdStockReceiving",
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
                    PartName = table.Column<string>(maxLength: 300, nullable: true),
                    PartListId = table.Column<long>(nullable: true),
                    PartListGradeId = table.Column<long>(nullable: true),
                    MaterialId = table.Column<long>(nullable: true),
                    Qty = table.Column<decimal>(nullable: true),
                    TransactionDatetime = table.Column<DateTime>(nullable: true),
                    InvoiceDetailsId = table.Column<long>(nullable: true),
                    WorkingDate = table.Column<DateTime>(nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 15, nullable: true),
                    Model = table.Column<string>(maxLength: 4, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdStockReceiving", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdStockReceiving");

            migrationBuilder.CreateTable(
                name: "ProdStockPart",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastCalDatetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    MaterialId = table.Column<long>(type: "bigint", nullable: true),
                    Model = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PartListId = table.Column<long>(type: "bigint", nullable: true),
                    PartName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PartNo = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    PartNoNormalized = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    PartNoNormalizedS4 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SupplierNo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    WorkingDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdStockPart", x => x.Id);
                });
        }
    }
}
