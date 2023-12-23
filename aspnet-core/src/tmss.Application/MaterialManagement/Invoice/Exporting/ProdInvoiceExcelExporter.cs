using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.Invoice.Exporting
{
    public class ProdInvoiceExcelExporter : NpoiExcelExporterBase, IProdInvoiceExcelExporter
    {
        public ProdInvoiceExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<ProdInvoiceDto> listdata)
        {
            return CreateExcelPackage(
                "Invoice.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Invoice");

                    AddHeader(
                        sheet,
                        ("Invoice No"),
                        ("Invoice Date"),
                        ("Bill No"),
                        ("Bill Date"),
                        ("Shipment No"),
                        ("Forwarder"),
                        ("Status")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.InvoiceNo,
                        _ => _.InvoiceDate,
                        _ => _.BillNo,
                        _ => _.BillDate,
                        _ => _.ShipmentNo,
                        _ => _.Forwarder,
                        _ => _.Status
                    );

                    for (var i = 0; i < 7; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }

        public FileDto ExportToFileDetails(List<ProdInvoiceDto> listdatadetails)
        {
            return CreateExcelPackage(
                "InvoiceDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Invoice Details");

                    AddHeader(
                        sheet,
                        ("Part No"),
                        ("Qty"),
                        ("Container No"),
                        ("Supplier No"),
                        ("Cost"),
                        ("Insurance"),
                        ("Freight"),
                        ("CIF"),
                        ("THC"),
                        ("TAX"),
                        ("TAX Rate"),
                        ("VAT"),
                        ("VAT Rate"),
                        ("Currency"),
                        ("Gross Weight"),
                        ("Carfamily Code"),
                        ("Part Name")
                    );

                    AddObjects(
                        sheet, 1, listdatadetails,
                        _ => _.PartNo,
                        _ => _.UsageQty,
                        _ => _.ContainerNo,
                        _ => _.SupplierNo,
                        _ => _.Cost,
                        _ => _.Insurance,
                        _ => _.Freight,
                        _ => _.Cif,
                        _ => _.Thc,
                        _ => _.Tax,
                        _ => _.TaxRate,
                        _ => _.Vat,
                        _ => _.VatRate,
                        _ => _.Currency,
                        _ => _.GrossWeight,
                        _ => _.CarfamilyCode,
                        _ => _.PartName
                    );

                    for (var i = 0; i < 17; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
