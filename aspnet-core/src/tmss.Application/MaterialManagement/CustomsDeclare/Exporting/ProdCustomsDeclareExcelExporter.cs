using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.CustomsDeclare.Exporting
{
    public class ProdCustomsDeclareExcelExporter : NpoiExcelExporterBase, IProdCustomsDeclareExcelExporter
    {
        public ProdCustomsDeclareExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {
        }

        public FileDto ExportToFile(List<ProdCustomsDeclareDto> listdata)
        {
            return CreateExcelPackage(
               "CustomsDeclare.xlsx",
               excelPackage =>
               {
                   var sheet = excelPackage.CreateSheet("Customs Declare");
                   AddHeader(
                               sheet,
                               ("Customs Declare No"),
                               ("Declare Date"),
                               ("Bill Of Lading No"),
                               ("Bill Date"),
                               ("Invoice No"),
                               ("Invoice Date"),
                               ("Forwarder"),
                               ("Status"),
                               ("Tax"),
                               ("Vat"),
                               ("Sum"),
                               ("Bill Id"),
                               ("Invoice Id")
                              );
                   AddObjects(
                        sheet, 1, listdata,
                               _ => _.CustomsDeclareNo,
                               _ => _.DeclareDate,
                               _ => _.BillOfLadingNo,
                               _ => _.BillDate,
                               _ => _.InvoiceNo,
                               _ => _.InvoiceDate,
                               _ => _.Forwarder,
                               _ => _.Status,
                               _ => _.Tax,
                               _ => _.Vat,
                               _ => _.SumCustomsDeclare,
                               _ => _.BillId,
                               _ => _.InvoiceId
                               );

                   for (var i = 0; i < 13; i++)
                   {
                       sheet.AutoSizeColumn(i);
                   }

               });
        }
    }
}
