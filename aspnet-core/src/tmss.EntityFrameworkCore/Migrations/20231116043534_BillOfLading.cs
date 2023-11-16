using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class BillOfLading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillOfLading",
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
                    BillofladingNo = table.Column<string>(maxLength: 20, nullable: true),
                    ShipmentId = table.Column<long>(nullable: true),
                    BillDate = table.Column<DateTime>(type: "date", nullable: true),
                    StatusCode = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillOfLading", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shipment",
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
                    ShipmentNo = table.Column<string>(maxLength: 10, nullable: true),
                    ShippingcompanyCode = table.Column<string>(maxLength: 10, nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 10, nullable: true),
                    Buyer = table.Column<string>(maxLength: 4, nullable: true),
                    FromPort = table.Column<string>(maxLength: 50, nullable: true),
                    ToPort = table.Column<string>(maxLength: 50, nullable: true),
                    ShipmentDate = table.Column<string>(maxLength: 10, nullable: true),
                    Etd = table.Column<DateTime>(type: "date", nullable: true),
                    Eta = table.Column<DateTime>(type: "date", nullable: true),
                    Ata = table.Column<DateTime>(type: "date", nullable: true),
                    OceanVesselName = table.Column<string>(maxLength: 30, nullable: true),
                    Atd = table.Column<DateTime>(type: "date", nullable: true),
                    Status = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillOfLading");

            migrationBuilder.DropTable(
                name: "Shipment");
        }
    }
}
