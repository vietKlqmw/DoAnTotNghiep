using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class MasterVehicleCBU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterVehicleCBU",
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
                    VehicleType = table.Column<string>(maxLength: 10, nullable: true),
                    Model = table.Column<string>(maxLength: 5000, nullable: true),
                    MarketingCode = table.Column<string>(maxLength: 50, nullable: true),
                    ProductionCode = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterVehicleCBU", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterVehicleCBU");
        }
    }
}
