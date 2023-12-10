using System.Collections.Generic;
using tmss.DataExporting.Excel.NPOI;
using tmss.Dto;
using tmss.Storage;

namespace tmss.MaterialManagement.ContainerWH.Exporting
{
    public class ProdContainerRentalWHPlanExcelExporter : NpoiExcelExporterBase, IProdContainerRentalWHPlanExcelExporter
    {
        public ProdContainerRentalWHPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<ProdContainerRentalWHPlanDto> listdata)
        {
            return CreateExcelPackage(
                "ContainerAtWarehouse.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContainerWarehouse");

                    AddHeader(
                        sheet,
                        ("Container No"),
                        ("Request Date"),
                        ("Request Time"),
                        ("Invoice No"),
                        ("Bill Of Lading No"),
                        ("Supplier No"),
                        ("Receive Date"),
                        ("Delivery Date"),
                        ("Transport"),
                        ("Status"),
                        ("Warehouse")
                    );

                    AddObjects(
                        sheet, 1, listdata,
                        _ => _.ContainerNo,
                        _ => _.RequestDate,
                        _ => _.InvoiceNo,
                        _ => _.BillofladingNo,
                        _ => _.SupplierNo,
                        _ => _.ReceiveDate,
                        _ => _.DeliveryDate,
                        _ => _.Transport,
                        _ => _.Status,
                        _ => _.Warehouse
                    );

                    for (var i = 0; i < 10; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
