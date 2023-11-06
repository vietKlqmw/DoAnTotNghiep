using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class MaterialMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterMaterial",
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
                    MaterialType = table.Column<string>(maxLength: 4, nullable: true),
                    MaterialCode = table.Column<string>(maxLength: 40, nullable: true),
                    Description = table.Column<string>(maxLength: 40, nullable: true),
                    MaterialGroup = table.Column<string>(maxLength: 9, nullable: true),
                    BaseUnitOfMeasure = table.Column<string>(maxLength: 3, nullable: true),
                    Plant = table.Column<string>(maxLength: 4, nullable: true),
                    StorageLocation = table.Column<string>(maxLength: 4, nullable: true),
                    ProductionGroup = table.Column<string>(maxLength: 3, nullable: true),
                    ProductionPurpose = table.Column<string>(maxLength: 2, nullable: true),
                    ReservedStock = table.Column<string>(maxLength: 2, nullable: true),
                    LotCode = table.Column<string>(maxLength: 10, nullable: true),
                    ProductionStorageLocation = table.Column<string>(maxLength: 4, nullable: true),
                    CostingLotSize = table.Column<decimal>(nullable: true),
                    ProductionVersion = table.Column<string>(maxLength: 4, nullable: true),
                    StandardPrice = table.Column<decimal>(nullable: true),
                    MovingPrice = table.Column<decimal>(nullable: true),
                    MaterialOrigin = table.Column<string>(maxLength: 1, nullable: true),
                    OriginGroup = table.Column<string>(maxLength: 4, nullable: true),
                    EffectiveDateFrom = table.Column<DateTime>(nullable: true),
                    EffectiveDateTo = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterMaterial", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterMaterial");
        }
    }
}
