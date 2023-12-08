using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class customsdeclare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProdCustomsDeclare",
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
                    CustomsDeclareNo = table.Column<string>(maxLength: 20, nullable: true),
                    DeclareDate = table.Column<DateTime>(nullable: true),
                    Tax = table.Column<decimal>(nullable: true),
                    Vat = table.Column<decimal>(nullable: true),
                    Status = table.Column<string>(maxLength: 4, nullable: true),
                    Forwarder = table.Column<string>(maxLength: 10, nullable: true),
                    BillId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdCustomsDeclare", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdCustomsDeclare");
        }
    }
}
