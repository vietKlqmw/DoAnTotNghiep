using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class MasterMaterialGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterFactory",
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
                    PlantCode = table.Column<string>(maxLength: 4, nullable: true),
                    PlantName = table.Column<string>(maxLength: 30, nullable: true),
                    BranchNo = table.Column<string>(maxLength: 4, nullable: true),
                    AddressLanguageEn = table.Column<string>(maxLength: 200, nullable: true),
                    AddressLanguageVn = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterFactory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterMaterialGroup",
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
                    Code = table.Column<string>(maxLength: 3, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterMaterialGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterStorageLocation",
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
                    PlantCode = table.Column<string>(maxLength: 4, nullable: true),
                    PlantName = table.Column<string>(maxLength: 30, nullable: true),
                    StorageLocation = table.Column<string>(maxLength: 4, nullable: true),
                    StorageLocationName = table.Column<string>(maxLength: 200, nullable: true),
                    AddressLanguageEn = table.Column<string>(maxLength: 200, nullable: true),
                    AddressLanguageVn = table.Column<string>(maxLength: 200, nullable: true),
                    Category = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterStorageLocation", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterFactory");

            migrationBuilder.DropTable(
                name: "MasterMaterialGroup");

            migrationBuilder.DropTable(
                name: "MasterStorageLocation");
        }
    }
}
