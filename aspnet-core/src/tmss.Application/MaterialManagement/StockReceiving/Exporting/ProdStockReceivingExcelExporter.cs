using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.StockReceiving.Exporting
{
    public class ProdStockReceivingExcelExporter : NpoiExcelExporterBase, IProdStockReceivingExcelExporter
    {
        public ProdStockReceivingExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<ProdStockReceivingDto> listdata)
        {
            return CreateExcelPackage(
                "ProdStockReceiving.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Stock Receiving");
                    AddHeader(
                        sheet,
                        ("Part No"),
                        ("Part Name"),
                        ("Carfamily Code"),
                        ("Warehouse"),
                        ("Supplier No"),
                        ("Container No"),
                        ("Qty"),
                        ("Actual Qty"),
                        ("Order Qty"),
                        ("Request Date"),
                        ("Request Status"),
                        ("Delivery Date"),
                        ("Invoice No Out")
                    );

                    AddObjects(
                         sheet, 1, listdata,
                         _ => _.PartNo,
                         _ => _.PartName,
                         _ => _.Model,
                         _ => _.Warehouse,
                         _ => _.SupplierNo,
                         _ => _.ContainerNo,
                         _ => _.Qty,
                         _ => _.ActualQty,
                         _ => _.OrderQty,
                         _ => _.RequestDate,
                         _ => _.RequestStatus,
                         _ => _.DeliveryDate,
                         _ => _.InvoiceNoOut
                    );

                    for (var i = 0; i < 13; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
