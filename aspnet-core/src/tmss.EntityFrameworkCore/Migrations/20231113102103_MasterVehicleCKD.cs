using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class MasterVehicleCKD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterCarfamily",
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
                    Code = table.Column<string>(maxLength: 10, nullable: true),
                    Name = table.Column<string>(maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterCarfamily", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterEngine",
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
                    MaterialCode = table.Column<string>(maxLength: 40, nullable: true),
                    ClassType = table.Column<string>(maxLength: 3, nullable: true),
                    ClassName = table.Column<string>(maxLength: 18, nullable: true),
                    TransmissionType = table.Column<string>(maxLength: 5, nullable: true),
                    EngineModel = table.Column<string>(maxLength: 10, nullable: true),
                    EngineType = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterEngine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterVehicleCKD",
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
                    Model = table.Column<string>(maxLength: 1, nullable: true),
                    LotCode = table.Column<string>(maxLength: 2, nullable: true),
                    Cfc = table.Column<string>(maxLength: 4, nullable: true),
                    Grade = table.Column<string>(maxLength: 2, nullable: true),
                    GradeName = table.Column<string>(maxLength: 50, nullable: true),
                    ModelCode = table.Column<string>(maxLength: 50, nullable: true),
                    VehicleId = table.Column<string>(maxLength: 2, nullable: true),
                    TransmissionType = table.Column<string>(maxLength: 2, nullable: true),
                    EngineType = table.Column<string>(maxLength: 10, nullable: true),
                    FuelType = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterVehicleCKD", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterCarfamily");

            migrationBuilder.DropTable(
                name: "MasterEngine");

            migrationBuilder.DropTable(
                name: "MasterVehicleCKD");
        }
    }
}
