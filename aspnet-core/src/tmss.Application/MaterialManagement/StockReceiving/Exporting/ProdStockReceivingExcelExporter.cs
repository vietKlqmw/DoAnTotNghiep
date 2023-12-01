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
                        ("Model"),
                        ("Supplier No"),
                        ("Container No"),
                        ("Invoice No"),
                        ("Qty"),
                        ("Par Name"),
                        ("Transaction Datetime"),
                        ("Working Date")
                    );

                    AddObjects(
                         sheet, 1, listdata,
                         _ => _.PartNo,
                         _ => _.Model,
                         _ => _.SupplierNo,
                         _ => _.ContainerNo,
                         _ => _.InvoiceNo,
                         _ => _.Qty,
                         _ => _.PartName,
                         _ => _.TransactionDatetime,
                         _ => _.WorkingDate
                    );

                    for (var i = 0; i < 9; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
