using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class bigupdatepartN1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualDevanningDate",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "BillofladingNo",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "DevanningDate",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "DevanningTime",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "GateInActualDateTime",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "GateInPlanTime",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "InvoiceNo",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "RequestTime",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "SealNo",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "BillDate",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "BillOfLadingNo",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "ContainerSize",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "DevanningDate",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "DevanningTime",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "GateInDate",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "GateInTime",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "InvoiceNo",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "LocationCode",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "LocationDate",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "PortDateActual",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "PortTransitDate",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "ReceiveTime",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "ReceivingPeriodId",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "RentalWhId",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "SealNo",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "TransitPortReqDate",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "TransitPortReqId",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "TransitPortReqTime",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "WhLocation",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "PlantCode",
                table: "MasterStorageLocation");

            migrationBuilder.DropColumn(
                name: "PlantName",
                table: "MasterStorageLocation");

            migrationBuilder.AddColumn<long>(
                name: "BillId",
                table: "ProdContainerRentalWHPlan",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveryDate",
                table: "ProdContainerRentalWHPlan",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "ProdContainerRentalWHPlan",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiveDate",
                table: "ProdContainerRentalWHPlan",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Warehouse",
                table: "ProdContainerRentalWHPlan",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BillId",
                table: "ProdContainerList",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "ProdContainerList",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StorageLocation",
                table: "MasterStorageLocation",
                maxLength: 2,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4)",
                oldMaxLength: 4,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillId",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "DeliveryDate",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "ReceiveDate",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "Warehouse",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "ProdContainerList");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDevanningDate",
                table: "ProdContainerRentalWHPlan",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillofladingNo",
                table: "ProdContainerRentalWHPlan",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DevanningDate",
                table: "ProdContainerRentalWHPlan",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DevanningTime",
                table: "ProdContainerRentalWHPlan",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GateInActualDateTime",
                table: "ProdContainerRentalWHPlan",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GateInPlanTime",
                table: "ProdContainerRentalWHPlan",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNo",
                table: "ProdContainerRentalWHPlan",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "RequestTime",
                table: "ProdContainerRentalWHPlan",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SealNo",
                table: "ProdContainerRentalWHPlan",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BillDate",
                table: "ProdContainerList",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillOfLadingNo",
                table: "ProdContainerList",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContainerSize",
                table: "ProdContainerList",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DevanningDate",
                table: "ProdContainerList",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DevanningTime",
                table: "ProdContainerList",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GateInDate",
                table: "ProdContainerList",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GateInTime",
                table: "ProdContainerList",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNo",
                table: "ProdContainerList",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationCode",
                table: "ProdContainerList",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LocationDate",
                table: "ProdContainerList",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PortDateActual",
                table: "ProdContainerList",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PortTransitDate",
                table: "ProdContainerList",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReceiveTime",
                table: "ProdContainerList",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReceivingPeriodId",
                table: "ProdContainerList",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RentalWhId",
                table: "ProdContainerList",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RequestId",
                table: "ProdContainerList",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SealNo",
                table: "ProdContainerList",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransitPortReqDate",
                table: "ProdContainerList",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TransitPortReqId",
                table: "ProdContainerList",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransitPortReqTime",
                table: "ProdContainerList",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhLocation",
                table: "ProdContainerList",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StorageLocation",
                table: "MasterStorageLocation",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlantCode",
                table: "MasterStorageLocation",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlantName",
                table: "MasterStorageLocation",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }
    }
}
