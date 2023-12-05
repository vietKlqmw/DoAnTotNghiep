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
                        ("Supplier No"),
                        ("Bill No"),
                        ("Bill Date"),
                        ("Shipment No"),
                        ("Freight"),
                        ("Insurance"),
                        ("Cif"),
                        ("Thc"),
                        ("Net Weight"),
                        ("Gross Weight"),
                        ("Currency"),
                        ("Quantity"),
                        ("Status")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.InvoiceNo,
                        _ => _.InvoiceDate,
                        _ => _.SupplierNo,
                        _ => _.BillNo,
                        _ => _.BillDate,
                        _ => _.ShipmentNo,
                        _ => _.FreightTotal,
                        _ => _.InsuranceTotal,
                        _ => _.Cif,
                        _ => _.ThcTotal,
                        _ => _.NetWeight,
                        _ => _.GrossWeight,
                        _ => _.Currency,
                        _ => _.Quantity,
                        _ => _.Status
                    );

                    for (var i = 0; i < 16; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }

        public FileDto ExportToFileDetails(List<ProdInvoiceDetailsDto> listdatadetails)
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
                        ("Freight"),
                        ("Insurance"),
                        ("CIF"),
                        ("THC"),
                        ("TAX"),
                        ("TAX Rate"),
                        ("VAT"),
                        ("VAT Rate"),
                        ("Carfamily Code"),
                        ("Part Net Weight"),
                        ("Packaging Date"),
                        ("Status"),
                        ("Part Name"),
                        ("Part Name Vn")
                    );

                    AddObjects(
                        sheet, 1, listdatadetails,
                        _ => _.PartNo,
                        _ => _.UsageQty,
                        _ => _.ContainerNo,
                        _ => _.SupplierNo,
                        _ => _.Freight,
                        _ => _.Insurance,
                        _ => _.Cif,
                        _ => _.Thc,
                        _ => _.Tax,
                        _ => _.TaxRate,
                        _ => _.Vat,
                        _ => _.VatRate,
                        _ => _.CarfamilyCode,
                        _ => _.PartNetWeight,
                        _ => _.PackagingDate,
                        _ => _.Status,
                        _ => _.PartName,
                        _ => _.PartnameVn
                    );

                    for (var i = 0; i < 18; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
