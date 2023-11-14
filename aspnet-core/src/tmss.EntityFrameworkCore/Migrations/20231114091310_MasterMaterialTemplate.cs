using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class MasterMaterialTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterEngine_T",
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
                    MaterialCode = table.Column<string>(maxLength: 40, nullable: true),
                    ClassType = table.Column<string>(maxLength: 3, nullable: true),
                    ClassName = table.Column<string>(maxLength: 18, nullable: true),
                    TransmissionType = table.Column<string>(maxLength: 5, nullable: true),
                    EngineModel = table.Column<string>(maxLength: 10, nullable: true),
                    EngineType = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterEngine_T", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterMaterial_T",
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
                    EffectiveDateTo = table.Column<DateTime>(nullable: true),
                    ProductionType = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterMaterial_T", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterVehicleCKD_T",
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
                    Model = table.Column<string>(maxLength: 1, nullable: true),
                    LotCode = table.Column<string>(maxLength: 2, nullable: true),
                    Cfc = table.Column<string>(maxLength: 4, nullable: true),
                    Grade = table.Column<string>(maxLength: 2, nullable: true),
                    GradeName = table.Column<string>(maxLength: 50, nullable: true),
                    ModelCode = table.Column<string>(maxLength: 50, nullable: true),
                    VehicleId = table.Column<string>(maxLength: 2, nullable: true),
                    CarSeries = table.Column<string>(maxLength: 18, nullable: true),
                    TransmissionType = table.Column<string>(maxLength: 2, nullable: true),
                    EngineType = table.Column<string>(maxLength: 10, nullable: true),
                    FuelType = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterVehicleCKD_T", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterEngine_T");

            migrationBuilder.DropTable(
                name: "MasterMaterial_T");

            migrationBuilder.DropTable(
                name: "MasterVehicleCKD_T");
        }
    }
}
