using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.ContainerInvoice.Exporting
{
    public class ProdContainerInvoiceExcelExporter : NpoiExcelExporterBase, IProdContainerInvoiceExcelExporter
    {
        public ProdContainerInvoiceExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<ProdContainerInvoiceDto> listdata)
        {
            return CreateExcelPackage(
                "ContainerInvoice.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerInvoice");

                    AddHeader(
                        sheet,
                        ("Container No"),
                        ("Invoice No"),
                        ("Supplier No"),
                        ("Seal No"),
                        ("Bill Of Lading No"),
                        ("Bill Date"),
                        ("Container Size"),
                        ("Plan Devanning Date"),
                        ("Actual Devanning Date"),
                        ("Status"),
                        ("Freight"),
                        ("Insurance"),
                        ("Tax"),
                        ("Amount"),
                        ("Tax Vnd"),
                        ("Vat Vnd")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.ContainerNo,
                        _ => _.InvoiceNo,
                        _ => _.SupplierNo,
                        _ => _.SealNo,
                        _ => _.BillofladingNo,
                        _ => _.BillDate,
                        _ => _.ContainerSize,
                        _ => _.PlanDevanningDate,
                        _ => _.ActualDevanningDate,
                        _ => _.Status,
                        _ => _.Freight,
                        _ => _.Insurance,
                        _ => _.Tax,
                        _ => _.Amount,
                        _ => _.TaxVnd,
                        _ => _.VatVnd
                    );

                    for (var i = 0; i < 16; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
