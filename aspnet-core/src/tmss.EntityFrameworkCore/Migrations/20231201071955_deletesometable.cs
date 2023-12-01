using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class deletesometable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterFuelType");

            migrationBuilder.DropTable(
                name: "MasterVehicleCKD");

            migrationBuilder.DropTable(
                name: "MasterVehicleCKD_T");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterFuelType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterFuelType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterVehicleCKD",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarSeries = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    Cfc = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EngineType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    GradeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    LotCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ModelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransmissionType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    VehicleId = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterVehicleCKD", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterVehicleCKD_T",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarSeries = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: true),
                    Cfc = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EngineType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ErrorDescription = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    FuelType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    GradeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Guid = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    LotCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    ModelCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TransmissionType = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    VehicleId = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterVehicleCKD_T", x => x.Id);
                });
        }
    }
}
