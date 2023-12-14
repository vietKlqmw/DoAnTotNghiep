using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class finaltable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdInvoiceStockOut",
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
                    InvoiceNoOut = table.Column<string>(maxLength: 20, nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true),
                    ListPartNo = table.Column<string>(maxLength: 12, nullable: true),
                    ListPartName = table.Column<string>(maxLength: 300, nullable: true),
                    ListCfc = table.Column<string>(maxLength: 4, nullable: true),
                    ListStockId = table.Column<string>(maxLength: 1000, nullable: true),
                    TotalOrderQty = table.Column<int>(nullable: true),
                    TotalAmount = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdInvoiceStockOut", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdInvoiceStockOut");
        }
    }
}
