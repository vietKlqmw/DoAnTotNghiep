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
                        ("Request Status"),
                        ("Container No"),
                        ("Supplier No"),
                        ("Status"),
                        ("Bill Of Lading No"),
                        ("Bill Date"),
                        ("Shipping Date"),
                        ("Port Date"),
                        ("Recieve Date"),
                        ("Invoice No"),
                        ("Transport"),
                        ("Freight"),
                        ("Insurance"),
                        ("C.I.F"),
                        ("TAX"),
                        ("Amount"),
                        ("Remark")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.RequestStatus,
                        _ => _.ContainerNo,
                        _ => _.SupplierNo,
                        _ => _.Status,
                        _ => _.BillOfLadingNo,
                        _ => _.BillDate,
                        _ => _.ShippingDate,
                        _ => _.PortDate,
                        _ => _.ReceiveDate,
                        _ => _.InvoiceNo,
                        _ => _.Transport,
                        _ => _.Freight,
                        _ => _.Insurance,
                        _ => _.Cif,
                        _ => _.Tax,
                        _ => _.Amount,
                        _ => _.Remark
                    );

                    for (var i = 0; i < 17; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
