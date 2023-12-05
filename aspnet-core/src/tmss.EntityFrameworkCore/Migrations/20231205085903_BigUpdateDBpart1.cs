using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class BigUpdateDBpart1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartListGradeId",
                table: "ProdStockReceiving");

            migrationBuilder.DropColumn(
                name: "ModuleNo",
                table: "ProdInvoiceDetails");

            migrationBuilder.DropColumn(
                name: "ListCaseNo",
                table: "ProdContainerTransitPortPlan_T");

            migrationBuilder.DropColumn(
                name: "ListLotNo",
                table: "ProdContainerTransitPortPlan_T");

            migrationBuilder.DropColumn(
                name: "ListCaseNo",
                table: "ProdContainerTransitPortPlan");

            migrationBuilder.DropColumn(
                name: "ListLotNo",
                table: "ProdContainerTransitPortPlan");

            migrationBuilder.DropColumn(
                name: "ListLotNo",
                table: "ProdContainerRentalWHPlan_T");

            migrationBuilder.DropColumn(
                name: "ListcaseNo",
                table: "ProdContainerRentalWHPlan_T");

            migrationBuilder.DropColumn(
                name: "ListLotNo",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "ListcaseNo",
                table: "ProdContainerRentalWHPlan");

            migrationBuilder.DropColumn(
                name: "GoodstypeCode",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "ListCaseNo",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "ListLotNo",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "OrdertypeCode",
                table: "ProdContainerList");

            migrationBuilder.DropColumn(
                name: "CostingLotSize",
                table: "MasterMaterial_T");

            migrationBuilder.DropColumn(
                name: "LotCode",
                table: "MasterMaterial_T");

            migrationBuilder.DropColumn(
                name: "OriginGroup",
                table: "MasterMaterial_T");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "MasterMaterial_T");

            migrationBuilder.DropColumn(
                name: "ProductionGroup",
                table: "MasterMaterial_T");

            migrationBuilder.DropColumn(
                name: "ProductionPurpose",
                table: "MasterMaterial_T");

            migrationBuilder.DropColumn(
                name: "ProductionStorageLocation",
                table: "MasterMaterial_T");

            migrationBuilder.DropColumn(
                name: "ProductionVersion",
                table: "MasterMaterial_T");

            migrationBuilder.DropColumn(
                name: "ReservedStock",
                table: "MasterMaterial_T");

            migrationBuilder.DropColumn(
                name: "CostingLotSize",
                table: "MasterMaterial");

            migrationBuilder.DropColumn(
                name: "LotCode",
                table: "MasterMaterial");

            migrationBuilder.DropColumn(
                name: "OriginGroup",
                table: "MasterMaterial");

            migrationBuilder.DropColumn(
                name: "Plant",
                table: "MasterMaterial");

            migrationBuilder.DropColumn(
                name: "ProductionGroup",
                table: "MasterMaterial");

            migrationBuilder.DropColumn(
                name: "ProductionPurpose",
                table: "MasterMaterial");

            migrationBuilder.DropColumn(
                name: "ProductionStorageLocation",
                table: "MasterMaterial");

            migrationBuilder.DropColumn(
                name: "ProductionVersion",
                table: "MasterMaterial");

            migrationBuilder.DropColumn(
                name: "ReservedStock",
                table: "MasterMaterial");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "RequestTime",
                table: "ProdContainerRentalWHPlan",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestDate",
                table: "ProdContainerRentalWHPlan",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "GateInActualDateTime",
                table: "ProdContainerRentalWHPlan",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "DevanningTime",
                table: "ProdContainerRentalWHPlan",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaterialOrigin",
                table: "MasterMaterial_T",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaterialOrigin",
                table: "MasterMaterial",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PartListGradeId",
                table: "ProdStockReceiving",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModuleNo",
                table: "ProdInvoiceDetails",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListCaseNo",
                table: "ProdContainerTransitPortPlan_T",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListLotNo",
                table: "ProdContainerTransitPortPlan_T",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListCaseNo",
                table: "ProdContainerTransitPortPlan",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListLotNo",
                table: "ProdContainerTransitPortPlan",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListLotNo",
                table: "ProdContainerRentalWHPlan_T",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListcaseNo",
                table: "ProdContainerRentalWHPlan_T",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "RequestTime",
                table: "ProdContainerRentalWHPlan",
                type: "time(7)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RequestDate",
                table: "ProdContainerRentalWHPlan",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "GateInActualDateTime",
                table: "ProdContainerRentalWHPlan",
                type: "datetime2(7)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "DevanningTime",
                table: "ProdContainerRentalWHPlan",
                type: "time(7)",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListLotNo",
                table: "ProdContainerRentalWHPlan",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListcaseNo",
                table: "ProdContainerRentalWHPlan",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoodstypeCode",
                table: "ProdContainerList",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListCaseNo",
                table: "ProdContainerList",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ListLotNo",
                table: "ProdContainerList",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrdertypeCode",
                table: "ProdContainerList",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaterialOrigin",
                table: "MasterMaterial_T",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostingLotSize",
                table: "MasterMaterial_T",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotCode",
                table: "MasterMaterial_T",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginGroup",
                table: "MasterMaterial_T",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "MasterMaterial_T",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionGroup",
                table: "MasterMaterial_T",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionPurpose",
                table: "MasterMaterial_T",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionStorageLocation",
                table: "MasterMaterial_T",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionVersion",
                table: "MasterMaterial_T",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReservedStock",
                table: "MasterMaterial_T",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaterialOrigin",
                table: "MasterMaterial",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CostingLotSize",
                table: "MasterMaterial",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LotCode",
                table: "MasterMaterial",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginGroup",
                table: "MasterMaterial",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Plant",
                table: "MasterMaterial",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionGroup",
                table: "MasterMaterial",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionPurpose",
                table: "MasterMaterial",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionStorageLocation",
                table: "MasterMaterial",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductionVersion",
                table: "MasterMaterial",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReservedStock",
                table: "MasterMaterial",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);
        }
    }
}
