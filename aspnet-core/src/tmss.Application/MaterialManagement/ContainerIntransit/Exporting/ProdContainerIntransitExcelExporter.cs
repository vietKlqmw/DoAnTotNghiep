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
                        ("Forwarder"),
                        ("Usage Qty"),
                        ("Status"),
                        ("Part No"),
                        ("Cfc")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.ContainerNo,
                        _ => _.SupplierNo,
                        _ => _.ShippingDate,
                        _ => _.PortDate,
                        _ => _.Forwarder,
                        _ => _.UsageQty,
                        _ => _.Status,
                        _ => _.PartNo,
                        _ => _.CarfamilyCode
                    );

                    for (var i = 0; i < 9; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
