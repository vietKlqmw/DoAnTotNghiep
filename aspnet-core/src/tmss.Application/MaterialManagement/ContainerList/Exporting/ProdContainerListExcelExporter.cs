using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.ContainerList.Exporting
{
    public class ProdContainerListExcelExporter : NpoiExcelExporterBase, IProdContainerListExcelExporter
    {
        public ProdContainerListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<ProdContainerListDto> listdata)
        {
            return CreateExcelPackage(
                "ContainerList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerList");

                    AddHeader(
                        sheet,
                        ("Container No"),
                        ("Status"),
                        ("Supplier No"),
                        ("Bill Of Lading No"),
                        ("Bill Date"),
                        ("Shipping Date"),
                        ("Port Date"),
                        ("Recieve Date"),
                        ("Invoice No"),
                        ("Cost"),
                        ("Insurance"),
                        ("Freight"),
                        ("C.I.F"),
                        ("TAX"),
                        ("Amount"),
                        ("Cfc"),
                        ("Part No"),
                        ("Part Name"),
                        ("Basic Of Measure"),
                        ("Warehouse"),
                        ("Transport"),
                        ("Remark")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.ContainerNo,
                        _ => _.Status,
                        _ => _.SupplierNo,
                        _ => _.BillOfLadingNo,
                        _ => _.BillDate,
                        _ => _.ShippingDate,
                        _ => _.PortDate,
                        _ => _.ReceiveDate,
                        _ => _.InvoiceNo,
                        _ => _.TotalAmount,
                        _ => _.Insurance,
                        _ => _.Freight,
                        _ => _.Cif,
                        _ => _.Tax,
                        _ => _.Amount,
                        _ => _.CarfamilyCode,
                        _ => _.PartNo,
                        _ => _.PartName,
                        _ => _.BaseUnitOfMeasure,
                        _ => _.Warehouse,
                        _ => _.Transport,
                        _ => _.Remark
                    );

                    for (var i = 0; i < 22; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
