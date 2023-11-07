using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.Master.UoM.Exporting
{
    public class MasterUnitOfMeasureExcelExporter : NpoiExcelExporterBase, IMasterUnitOfMeasureExcelExporter
    {
        public MasterUnitOfMeasureExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<MasterUnitOfMeasureDto> listdata)
        {
            return CreateExcelPackage(
                "MasterUnitOfMeasure.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("UnitOfMeasure");

                    AddHeader(
                        sheet,
                        ("Code"),
                        ("Name")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.Code,
                        _ => _.Name
                    );

                    for (var i = 0; i < 2; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
