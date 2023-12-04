using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class deletefactory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterFactory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterFactory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressLanguageEn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AddressLanguageVn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BranchNo = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    PlantCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PlantName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterFactory", x => x.Id);
                });
        }
    }
}
