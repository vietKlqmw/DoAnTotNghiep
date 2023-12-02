using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.BillOfLading.Exporting
{
    public class ProdBillOfLadingExcelExporter : NpoiExcelExporterBase, IProdBillOfLadingExcelExporter
    {
        public ProdBillOfLadingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<ProdBillOfLadingDto> listdata)
        {
            return CreateExcelPackage(
                "BillOfLading.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Bill Of Lading");

                    AddHeader(
                        sheet,
                        ("Bill Of Lading No"),
                        ("Shipment No"),
                        ("Bill Date"),
                        ("Status")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.BillofladingNo,
                        _ => _.ShipmentNo,
                        _ => _.BillDate,
                        _ => _.StatusCode

                    );

                    for (var i = 0; i < 4; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
