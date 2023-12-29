using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class importorderpart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdOrderPart_T",
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
                    PartNo = table.Column<string>(maxLength: 15, nullable: true),
                    PartName = table.Column<string>(maxLength: 500, nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 10, nullable: true),
                    CarfamilyCode = table.Column<string>(maxLength: 4, nullable: true),
                    Remark = table.Column<string>(maxLength: 5000, nullable: true),
                    Qty = table.Column<int>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: true),
                    ErrorDescription = table.Column<string>(maxLength: 5000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdOrderPart_T", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdOrderPart_T");
        }
    }
}
