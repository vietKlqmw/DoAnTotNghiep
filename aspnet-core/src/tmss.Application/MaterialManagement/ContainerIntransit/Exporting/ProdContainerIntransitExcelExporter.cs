using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.ContainerIntransit.Exporting
{
    public class ProdContainerIntransitExcelExporter : NpoiExcelExporterBase, IProdContainerIntransitExcelExporter
    {
        public ProdContainerIntransitExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<ProdContainerIntransitDto> listdata)
        {
            return CreateExcelPackage(
                "ContainerIntransit.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Container Intransit");

                    AddHeader(
                        sheet,
                        ("Container No"),
                        ("Supplier No"),
                        ("Shipping Date"),
                        ("Port Date"),
                        ("Transaction Date"),
                        ("Forwarder")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.ContainerNo,
                        _ => _.SupplierNo,
                        _ => _.ShippingDate,
                        _ => _.PortDate,
                        _ => _.TransactionDate,
                        _ => _.Forwarder
                    );

                    for (var i = 0; i < 6; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
