using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.InvoiceStock.Exporting
{
    public class ProdInvoiceStockOutExcelExporter : NpoiExcelExporterBase, IProdInvoiceStockOutExcelExporter
    {
        public ProdInvoiceStockOutExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ProdInvoiceStockOutDto> listdata)
        {
            return CreateExcelPackage(
              "InvoiceStockOut.xlsx",
              excelPackage =>
              {
                  var sheet = excelPackage.CreateSheet("InvoiceStockOut");

                  AddHeader(
                      sheet,
                      ("Invoice No Out"),
                      ("Invoice Date"),
                      ("Status"),
                      ("List Cfc"),
                      ("List Part No"),
                      ("List Part Name"),
                      ("Total Order Qty"),
                      ("Total Amount"),
                      ("Warehouse")
                  );

                  AddObjects(
                      sheet, 1, listdata,
                      _ => _.InvoiceNoOut,
                      _ => _.InvoiceDate,
                      _ => _.Status,
                      _ => _.ListCfc,
                      _ => _.ListPartNo,
                      _ => _.ListPartName,
                      _ => _.TotalOrderQty,
                      _ => _.TotalAmount,
                      _ => _.Warehouse
                  );

                  for (var i = 0; i < 9; i++)
                  {
                      sheet.AutoSizeColumn(i);
                  }
              });
        }
    }
}
