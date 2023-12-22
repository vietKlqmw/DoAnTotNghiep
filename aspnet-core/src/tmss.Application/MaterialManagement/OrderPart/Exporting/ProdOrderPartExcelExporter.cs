using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.OrderPart.Exporting
{
    public class ProdOrderPartExcelExporter : NpoiExcelExporterBase, IProdOrderPartExcelExporter
    {
        public ProdOrderPartExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ProdOrderPartDto> listdata)
        {
            return CreateExcelPackage(
                "ListOrderPart.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("List Order Part");

                    AddHeader(
                        sheet,
                        ("Part No"),
                        ("Part Name"),
                        ("Carfamily Code"),
                        ("Supplier No"),
                        ("Order Date"),
                        ("Qty"),
                        ("Amount Unit"),
                        ("Total Amount"),
                        ("Status"),
                        ("Container No"),
                        ("Shipment No"),
                        ("Remark")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.PartNo,
                        _ => _.PartName,
                        _ => _.CarfamilyCode,
                        _ => _.SupplierNo,
                        _ => _.OrderDate,
                        _ => _.Qty,
                        _ => _.AmountUnit,
                        _ => _.TotalAmount,
                        _ => _.Status,
                        _ => _.ContainerNo,
                        _ => _.ShipmentNo,
                        _ => _.Remark
                    );

                    for (var i = 0; i < 12; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
