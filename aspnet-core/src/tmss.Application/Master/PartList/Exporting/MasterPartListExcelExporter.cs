using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.Master.PartList.Exporting
{
    public class MasterPartListExcelExporter : NpoiExcelExporterBase, IMasterPartListExcelExporter
    {
        public MasterPartListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<MasterPartListDto> listdata)
        {
            return CreateExcelPackage(
                "PartList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PartList");

                    AddHeader(
                        sheet,
                        ("Part No"),
                        ("Part Name"),
                        ("Supplier No"),
                        ("Carfamily Code"),
                        ("Base Unit Of Measure"),
                        ("Standard Price"),
                        ("Moving Price"),
                        ("Start Production Month"),
                        ("End Production Month"),
                        ("Remark")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.PartNo,
                        _ => _.PartName,
                        _ => _.SupplierNo,
                        _ => _.CarfamilyCode,
                        _ => _.BaseUnitOfMeasure,
                        _ => _.StandardPrice,
                        _ => _.MovingPrice,
                        _ => _.StartProductionMonth,
                        _ => _.EndProductionMonth,
                        _ => _.Remark
                    );

                    for (var i = 0; i < 10; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
