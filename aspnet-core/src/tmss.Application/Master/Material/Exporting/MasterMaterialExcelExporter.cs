﻿using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.Master.Material.Exporting
{
    public class MasterMaterialExcelExporter : NpoiExcelExporterBase, IMasterMaterialExcelExporter
    {
        public MasterMaterialExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<MasterMaterialDto> listdata)
        {
            return CreateExcelPackage(
                "MasterMaterial.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("MaterialMaster");

                    AddHeader(
                        sheet,
                        ("Material Type"),
                        ("Material Code"),
                        ("Description"),
                        ("Material Group"),
                        ("Base Unit Of Measure"),
                        ("Storage Location"),
                        ("Production Type"),
                        ("Standard Price"),
                        ("Moving Price"),
                        ("Material Origin"),
                        ("Effective Date From"),
                        ("Effective Date To")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.MaterialType,
                        _ => _.MaterialCode,
                        _ => _.Description,
                        _ => _.MaterialGroup,
                        _ => _.BaseUnitOfMeasure,
                        _ => _.StorageLocation,
                        _ => _.ProductionType,
                        _ => _.StandardPrice,
                        _ => _.MovingPrice,
                        _ => _.MaterialOrigin,
                        _ => _.EffectiveDateFrom,
                        _ => _.EffectiveDateTo
                    );

                    for (var i = 0; i < 12; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
