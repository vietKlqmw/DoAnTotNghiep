﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace tmss.Migrations
{
    public partial class addPartList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterPartList",
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
                    PartNo = table.Column<string>(maxLength: 50, nullable: true),
                    PartName = table.Column<string>(maxLength: 10, nullable: true),
                    SupplierNo = table.Column<string>(maxLength: 50, nullable: true),
                    SupplierId = table.Column<long>(nullable: true),
                    MaterialId = table.Column<long>(nullable: true),
                    CarfamilyCode = table.Column<string>(maxLength: 4, nullable: true),
                    StartProductionMonth = table.Column<DateTime>(nullable: true),
                    EndProductionMonth = table.Column<DateTime>(nullable: true),
                    Remark = table.Column<string>(maxLength: 5000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterPartList", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterPartList");
        }
    }
}
